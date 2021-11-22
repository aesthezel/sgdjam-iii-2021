using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using Code.Data;
using Code.Services;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Assertions;

namespace Code.Hero
{
    
    public class PlayerController2D : MonoBehaviour
    {
        [BoxGroup("--- Player Stats ---")]
        [SerializeField] private FloatData speed;
        [BoxGroup("--- Player Stats ---")]
        [SerializeField] private IntData lifes;
        
        [BoxGroup("Movement", centerLabel: true)]
        [SerializeField] private float moveVelocity;
        [BoxGroup("Movement")]
        [SerializeField] private LayerMask whatIsWall;
        
        [BoxGroup("Physics", centerLabel: true)]
        [SerializeField] private float skinWidth = 0.0015f;
        [BoxGroup("Physics")]
        [SerializeField] private int faceCheckers = 4;
        [BoxGroup("Physics")]
        [SerializeField] private int groundCheckers = 4;
        [BoxGroup("Physics")]
        [SerializeField] private Collider2D hitCollider;
                
        
        [BoxGroup("Jumping", centerLabel: true)]
        [SerializeField] private float jumpVel;
        [BoxGroup("Jumping")]
        [SerializeField] private float gravity = -10;
        [BoxGroup("Jumping")]
        [SerializeField] private float gravityModifier = 2f;
        [BoxGroup("Jumping")]
        [SerializeField] private LayerMask whatIsGround;
        
        [BoxGroup("-- Looking --")]
        [SerializeField] private Transform bodyPart;
        
        [BoxGroup("--- Animator ---")] 
        [SerializeField] private Animator bodyAnimator;
        [BoxGroup("--- Animator ---")] 
        [SerializeField] private string movementAnimationVar, 
                                        runningAnimationVar,
                                        jumpAnimationVar, 
                                        dashAnimationVar,
                                        hitAnimationVar,
                                        groundedAnimationVar, 
                                        fallAnimationVar;
        
        [BoxGroup("Checkpoint")]
        [SerializeField] private LayerMask whatIsCheckpoint;

        [SerializeField] private CinemachineVirtualCamera cCamera;
        private CinemachineFramingTransposer _transposer;
        
        // Movement
        private Vector2 _currentVelocity;
        private Action<float> _onDirectionChange;
        private Coroutine _moveCoroutine;

        // Facing
        private float _facingDirection;
        private float _currentGravityModifier = 2f;
        private bool _grounded;

        // Physics
        private Collider2D _myCollider;
        private float _heightGap;
        private float _widthGap;
        private ColliderOrigins _origins;
        private PlayerCollisions _playerCollisions;
        
        // Input
        private PlayerReceiver _receiver;
        private InputMapper _mapper;
        private Vector2 _playerOneInput;
        private Vector2 _playerTwoInput;
        private bool _canMove = true;
        private bool _canGetHit = true;
        private Vector2 _playerInput;

        // Jump
        private bool _jumping;
        private bool _jumpCompleted;
        private bool _canDash;
        private bool _jumpStrike;
        
        // Pool
        private MainPoolerService pooler;

        // -------------------
        // Properties
        // -------------------
        private Vector2 PlayerOneInput
        {
            get => _playerOneInput;
            set => _playerOneInput = value;
        }
        
        private Vector2 PlayerTwoInput
        {
            get => _playerTwoInput; 
            set => _playerTwoInput = value;
        }

        public bool Grounded
        {
            get => _grounded;
            private set
            {
                // Siempre que deje de estar en el suelo que determine donde estaba
                if (_grounded && !value)
                    JumpStartHeight = transform.position.y;

                // If player touches the ground...
                if (!_grounded && value)
                {
                    ResetJumpValues();
                    
                    _canDash = true;
                    
                    // Set the player on exact ground position
                    var info = MultiRayEmiter(_origins.bottomLeft, Vector2.down, 0.08f, whatIsGround);

                    if (info.hasHit.Contains(true))
                        transform.Translate(Vector2.down * info.hitDistances.Min());
                    
                    // Comprobar si ha saltado desde muy alto
                    _jumpStrike = JumpStartHeight - transform.position.y > 5f;
                    
                    if (_jumpStrike)
                    {
                        var jumpEffect = pooler.Pooler.GetByID("HighJump");
                        jumpEffect.transform.position =
                            _myCollider.bounds.center + Vector3.down * _myCollider.bounds.extents.y;
                        _jumpStrike = false;
                        StartCoroutine(DeactivateDelay(jumpEffect, 0.5f));
                    }
                    
                    _currentGravityModifier = gravityModifier;
                    _currentVelocity.y = _currentVelocity.y < 0 ? 0 : _currentVelocity.y;
                }
                _grounded = value;
            }
        }
        
        // -------------------
        // ReadOnly Properties
        // -------------------
        public PlayerCollisions PlayerCollisions => _playerCollisions;
        public Vector2 CurrentVelocity => _currentVelocity;
        public float FacingDirection => _facingDirection;
        public bool Dashing { get; private set; }
        
        public float JumpStartHeight { get; private set; }
        public Vector2 LastCheckpoint { get; private set; }

        // -------------------
        // UNITY METHODS
        // -------------------
        private void Awake()
        {
            _myCollider = GetComponent<Collider2D>();
            _mapper = GetComponent<InputMapper>();
            _receiver = GetComponent<PlayerReceiver>();
            _transposer = cCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Start()
        {
            pooler = ServiceLocator.Instance.ObtainService<MainPoolerService>();
            // At least we need 2 checkers for collision detection
            groundCheckers = Mathf.Clamp(groundCheckers, 2, int.MaxValue);
            faceCheckers = Mathf.Clamp(faceCheckers, 2, int.MaxValue);

            // Calculate direction for the first time, since player hasn't made any direction change yet
            CalculateFacingDirection(Mathf.Sign(transform.localScale.x));
            
            // Update players' input direction
            _mapper.onMove += UpdateMindInput;
            lifes.OnValueChange += GetHit;
            
            // Subscribe jump methods to combined event system
            SubscribeJumpActions();
            SubscribeDashActions();
        }

        private void Update()
        {
            if(_canMove)
                SetPlayerInput();
            
            UpdateRayOrigins();
            
            CheckGrounded();

            if (!Grounded)
            {
                CheckCeilCollision();
                ApplyGravity();
            }
            
            //CheckFaceCollisions();
            
            if(_currentVelocity.y < 0)
                _transposer.m_DeadZoneHeight = 0;
            else
                _transposer.m_DeadZoneHeight = 0.6f;

                // Move the amount computed by our physics
            Move(_currentVelocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            GroundedAnimation();
            UpdateMoveAnimation();
        }


        // -------------
        // Input
        // -------------
        private void UpdateMindInput(int mindId, Vector2 inputVector)
        {
            if (mindId == 0)
                PlayerOneInput = inputVector;
            else if (mindId == 1)
                PlayerTwoInput = inputVector;
        }
        
        private void SetPlayerInput()
        {
            var newDir = (_playerTwoInput + _playerOneInput) / 2f;
            
            if (_playerInput.x != newDir.x)
            {
                CalculateFacingDirection(newDir.x);
                _currentVelocity.x = moveVelocity * newDir.x;
            }

            _playerInput = newDir;
        }
        

        //---------------------------------
        // SUBSCRIPTIONS TO INPUT ACTIONS
        //---------------------------------
        private void SubscribeJumpActions()
        {
            var bJumAction = _mapper.ActionMapper.TryGetValue("Jump", out var jumpEvents);
            Assert.IsTrue(bJumAction, "$Jump input action not found in $InputMapper");
            
            jumpEvents.checker += PerformNormalJump;
            jumpEvents.ok += PerformSuperJump;
            jumpEvents.finished += () => Debug.Log("Jump Finished");
        }
        
        private void SubscribeDashActions()
        {
            var bDashAction = _mapper.ActionMapper.TryGetValue("Dash", out var dashEvents);
            Assert.IsTrue(bDashAction, "$Jump input action not found in $InputMapper");
            
            dashEvents.checker += CheckDash;
            dashEvents.ok += Dash;
            //dashEvents.finished += DashFinished; NOT IMPLEMENTED
        }
        
        
        // -------------
        // Jump
        // -------------
        private bool PerformNormalJump(float time)
        {
            if (Grounded)
            {
                _jumping = true;
                _currentVelocity.y = jumpVel/2;
                JumpAnimation();
            }

            return Grounded;
        }

        private void PerformSuperJump(float elapsedTime)
        {
            _currentGravityModifier = gravityModifier;
            
            if(elapsedTime < 0.3f)
                _currentVelocity.y = jumpVel;
            else
                _currentVelocity.y = jumpVel * 2f / 3f;

            //JumpAnimation();

            _jumpCompleted = true;
        }
        
        private void ResetJumpValues()
        {
            if (_jumping && !_jumpCompleted)
                _mapper.ActionMapper["Jump"].finished?.Invoke();

            _jumping = false;
            _jumpCompleted = false;
        }
        
        
        // -------------
        // Dash
        // -------------
        private bool CheckDash(float time)
        {
            return _canDash;
        }
        
        private void Dash(float elapsedTime)
        {
            _canMove = false;
            _canDash = false;
            Dashing = true;
            _currentVelocity = new Vector2(18f * _facingDirection, 0);
            hitCollider.enabled = false;

            // Animate
            bodyAnimator.SetTrigger(dashAnimationVar);
            
            StartCoroutine(StopDash());
        }

        private IEnumerator StopDash()
        {
            var elapse = 0f;
            
            while (elapse < 0.25f)
            {
                elapse += Time.deltaTime;
                
                _currentGravityModifier = 0;
                
                if (_playerCollisions.leftCollision || _playerCollisions.rightCollision)
                    _currentVelocity.x = 0;
                
                yield return new WaitForEndOfFrame();
            }

            DashFinished();
        }
        
        private void DashFinished()
        {
            hitCollider.enabled = true;
            if(Grounded) 
                _canDash = true;
            _canMove = true;
            _currentVelocity.x = _playerInput.x * moveVelocity;
            _currentGravityModifier = gravityModifier;
            Dashing = false;

        }

        // -------------
        // Hit
        // -------------
        public void GetHit()
        {
            if (_canGetHit)
                StartCoroutine(CantGetHit(0.2f));
        }

        private IEnumerator CantGetHit(float time)
        {
            var elapsedTime = 0f;
            _canGetHit = false;
            bodyAnimator.SetTrigger(hitAnimationVar);
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        // -------------
        // Facing
        // -------------
        private void CalculateFacingDirection(float value)
        {
            if(value != 0) 
                _facingDirection = value > 0 ? 1 : -1;
            
            var scale = transform.localScale;
            scale.x = (int)_facingDirection == 1 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        
        // -------------
        // Animation
        // -------------
        #region ANIMATIONS
        
        private void UpdateMoveAnimation()
        {
            // Movement Animations
            bodyAnimator.SetFloat(movementAnimationVar, Mathf.Abs(_playerInput.x));
            bodyAnimator.SetBool(runningAnimationVar, Mathf.Abs(_playerInput.x) > 0.5f);
            bodyAnimator.SetFloat(fallAnimationVar, _currentVelocity.y);
        }

        private void JumpAnimation() => bodyAnimator.SetTrigger(jumpAnimationVar);
        
        private void GroundedAnimation() => bodyAnimator.SetBool(groundedAnimationVar, Grounded);
        
        # endregion
        
        // -------------
        // Movement
        // -------------
        #region PLAYER MOVEMENT

        private void Move(Vector2 delta)
        {
            // Clamp X if colliding with walls
            if (_playerCollisions.leftCollision || _playerCollisions.rightCollision)
                delta.x = 0f;
            
            // Don't let player break ceil or ground collisions 
            if (!Grounded)
                ClampTopDistance(ref delta);
            
            ClampOnFalling(ref delta);
            ClampFaceDirection(ref delta);
            
            // Move clamped delta
            transform.Translate(delta);
        }

        public void PushBack(float vel)
        {
            _canMove = false;
            _currentVelocity.x = vel * FacingDirection * -1;
            StartCoroutine(PushCoroutine());
        }

        private IEnumerator PushCoroutine()
        {
            var elapsed = 0f;
            
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            _currentVelocity.x = _playerInput.x * moveVelocity;
            _canMove = true;
        }

        #endregion

        // --------------------------
        // Checkpoint
        // --------------------------
        #region  CHECKPOINT

        public void BackToCheckpoint()
        {
            _currentVelocity = Vector2.zero;
            _jumpStrike = false;
            transform.position = LastCheckpoint;
            var effect = ServiceLocator.Instance.ObtainService<MainPoolerService>().Pooler.GetByID("Respawn");
            effect.transform.position = LastCheckpoint;
            StartCoroutine(DeactivateDelay(effect, 0.2f)); 
        }

        #endregion
        
        private IEnumerator DeactivateDelay(GameObject go, float time)
        {
            float elapsed = 0;
            
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            go.SetActive(false);
        }
        
        // --------------------------
        // CUSTOM PHYSICS
        // --------------------------
        # region CUSTOM PLAYER CONTROLLER PHYSICS
        
        private void ApplyGravity()
        {
            _currentVelocity.y += gravity * Time.deltaTime * _currentGravityModifier;
            _currentGravityModifier += Time.deltaTime;
        }
        
        private MultiRayInfo MultiRayEmiter(Vector2 origin, Vector2 direction, float rayLength, LayerMask mask)
        {
            var hitResult = new MultiRayInfo();
            var horizontalDirection = direction == Vector2.right || direction == Vector2.left;
            var moveDirection = new Vector2(Mathf.Abs(direction.y), Mathf.Abs(direction.x));
            var gap = horizontalDirection ? _widthGap : _heightGap;
            var rayAmount = horizontalDirection ? faceCheckers : groundCheckers;

            for (var i = 0; i < rayAmount; i++)
            {
                var from = origin + moveDirection * (gap * i + skinWidth);
                RaycastHit2D hit = Physics2D.Raycast(from, direction, rayLength, mask);
                Debug.DrawLine(from, from + direction * rayLength);
                
                if (hit) hitResult.AddRayInfo(true, hit.distance);
                else     hitResult.AddRayInfo(false, float.MaxValue);
            }
            
            return hitResult;
        }
        
        private void UpdateRayOrigins()
        {
            var col = _myCollider.bounds;
            
            // Update gap If the collider is changed during an animation it will keep working fine
            _widthGap = (col.size.y - 2 * skinWidth) / (faceCheckers - 1);
            _heightGap = (col.size.x - 2 * skinWidth) / (groundCheckers - 1);
            
            // The 4 corners of the collider
            _origins.topRight = new Vector2(col.max.x, col.max.y);
            _origins.bottomRight = new Vector2(col.max.x, col.min.y);
            _origins.topLeft = new Vector2(col.min.x, col.max.y);
            _origins.bottomLeft = new Vector2(col.min.x, col.min.y);
        }
        
        private void CheckGrounded()
        {
            var info = MultiRayEmiter(_origins.bottomLeft, Vector2.down, 0.05f, whatIsGround);
            Grounded = info.hasHit.Contains(true);
            _playerCollisions.groundCollision = Grounded;
            CheckpointUpdate();
        }
        
        private void CheckpointUpdate()
        {
            var info = MultiRayEmiter(_origins.bottomLeft, Vector2.down, 0.05f, whatIsCheckpoint);
            
            if (info.hasHit.All(a => a))
                LastCheckpoint = transform.position;
        }

        private void CheckCeilCollision()
        {
            var topInfo = MultiRayEmiter(_origins.topLeft, Vector2.up, 0.05f, whatIsWall);
            
            _playerCollisions.topCollision = topInfo.hasHit.Contains(true);
            
            if (_playerCollisions.topCollision && _currentVelocity.y > 0)
                _currentVelocity.y = 0;
        }

        private void ClampTopDistance(ref Vector2 delta)
        {
            var length = Mathf.Abs(_currentVelocity.y);
            var ceilInfo = MultiRayEmiter(_origins.topLeft, Vector2.up, length, whatIsWall);
            if (ceilInfo.hasHit.Contains(true))
                delta.y = Mathf.Clamp(delta.y, float.MinValue, ceilInfo.hitDistances.Min());
        }
        
        private void ClampOnFalling(ref Vector2 delta)
        {
            var length = Mathf.Abs(_currentVelocity.y);
            var groundInfo = MultiRayEmiter(_origins.bottomLeft, Vector2.down, length, whatIsGround);
            
            if (groundInfo.hasHit.Contains(true))
                delta.y = Mathf.Clamp(delta.y, -groundInfo.hitDistances.Min(), float.MaxValue);
        }
        
        private void ClampFaceDirection(ref Vector2 delta)
        {
            var faceOrigin = _facingDirection > 0 ? _origins.bottomRight : _origins.bottomLeft;
            var faceDirection = Vector2.right * _facingDirection;
            var faceHits = MultiRayEmiter(faceOrigin, faceDirection, Mathf.Abs(_currentVelocity.x), whatIsWall);

            if (faceHits.hasHit.Contains(true))
            {
                var dist = faceHits.hitDistances.Min();
                if (dist <= 0.05)
                {
                    _playerCollisions.leftCollision = (int)_facingDirection == -1;
                    _playerCollisions.rightCollision = (int)_facingDirection == 1;
                }

                delta.x = Mathf.Clamp(delta.x, -dist, dist);
            }
            else
            {
                _playerCollisions.ResetFaceCollisions();
            }
        }
        
        #endregion
    }
}