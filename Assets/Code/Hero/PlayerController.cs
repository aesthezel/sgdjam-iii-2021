using System.Collections;
using Code.Data;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Code.Hero
{
    [RequireComponent(typeof(InputMapper))]
    public class PlayerController : MonoBehaviour
    {
        [BoxGroup("--- Player Stats ---")]
        [SerializeField] private FloatData speed;
        [BoxGroup("--- Player Stats ---")]
        [SerializeField] private IntData lifes;

        [BoxGroup("-- Ground Check --")]
        [SerializeField] private LayerMask whatIsGround;
        [BoxGroup("-- Ground Check --")]
        [SerializeField] private LayerMask whatIsNotCheckpoint;
        [BoxGroup("-- Ground Check --")]
        [SerializeField] private LayerMask whatIsWall;
        [BoxGroup("-- Ground Check --")]
        [SerializeField] private Transform[] groundCheckers;

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
            groundedAnimationVar;
        
        [BoxGroup("--- Skill Configs ---")]
        
        [TitleGroup("--- Skill Configs ---/Dash")]
        [SerializeField] private FloatData dashTime;
        [TitleGroup("--- Skill Configs ---/Dash")]
        [SerializeField] private FloatData dashDistance;
        [TitleGroup("--- Skill Configs ---/Dash")]
        [SerializeField] private FloatData dashSuccessMaxTime;
        [TitleGroup("--- Skill Configs ---/Dash")]
        [SerializeField] private Collider2D hitChecker;
        
        [TitleGroup("--- Skill Configs ---/Jumping")]
        [SerializeField] private FloatData coyoteTimeValue;
        [TitleGroup("--- Skill Configs ---/Jumping")]
        [SerializeField] private FloatData gravityModifier;
        [TitleGroup("--- Skill Configs ---/Jumping")]
        [SerializeField] private FloatData jumpForceAlone;
        [TitleGroup("--- Skill Configs ---/Jumping")]
        [SerializeField] private FloatData jumpForceCombined;
        
        // Components
        private Rigidbody2D _myRigidBody;
        private InputMapper _mapper;
        private PlayerReceiver _receiver;
        
        // Checkpoint
        private Vector3 _lastFullyOnGround;
        
        // Jumping
        private bool _coyoteCheck;
        private bool _isGrounded;
        private bool _jumping;
        private bool _jumpCompleted;
        private bool _canDoubleJump;
        private int _jumpCount;
        private float _airTime;
        
        // Movement
        private Vector2 _playerOneMovement;
        private Vector2 _playerTwoMovement;
        
        // dash
        private bool _canDash;
        
        // facing
        private bool _facingRight = true;
        
        public bool IsGrounded
        {
            get => _isGrounded;
            private set
            {
                // From air to ground
                if (!_isGrounded && value)
                    OnPlayerTouchesGround();

                // From ground to air
                else if (_isGrounded && !value && !_coyoteCheck && !_jumping)
                    StartCoroutine(CoyoteTime(coyoteTimeValue.Value));
                
                _isGrounded = value;
            }
        }
        public float HorizontalDirection { get; private set; }
        public IntData Lifes => lifes;
        public bool FacingRight
        {
            get => _facingRight;
            set 
            {
                Vector3 theScale = bodyPart.localScale;
                theScale.x = value == false ? -1 : 1;
                bodyPart.localScale = theScale;
                _facingRight = value;
            }
        }
        
        public bool CanMove { get; set; } = true;

        //----------------
        // UNITY METHODS
        //----------------
        private void Awake()
        {
            _myRigidBody = GetComponent<Rigidbody2D>();
            _mapper = GetComponent<InputMapper>();
            _receiver = GetComponent<PlayerReceiver>();
        }

        private void Start()
        {
            SubscribeJumpActions();
            SubscribeDashActions();
            SubscribeMovementActions();
        }

        private void Update()
        {
            IsGrounded = CheckGrounded();
            
            // Evita que se quede enganchado en las paredes
            // var hit = Physics2D.Raycast(transform.position, bodyPart.right, 0.2f, whatIsWall);
            // if (hit.collider != null && _jumping)
            // {
            //     if (_facingRight)
            //     {
            //         if(_playerOneMovement.x > 0)
            //             _playerOneMovement.x = 0; 
            //         if(_playerTwoMovement.x > 0)
            //             _playerTwoMovement.x = 0;
            //     }
            //     else
            //     {
            //         if(_playerOneMovement.x < 0)
            //             _playerOneMovement.x = 0; 
            //         if(_playerTwoMovement.x < 0)
            //             _playerTwoMovement.x = 0;
            //     }
            // }
            
            if (!_isGrounded)
            {
                _airTime += Time.deltaTime * gravityModifier.Value;
                _myRigidBody.velocity += Vector2.down * _airTime;
            }
            
            // Velocidad en Y acotada
            var velocity = _myRigidBody.velocity;
            _myRigidBody.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -10, 100));
        }

        private void FixedUpdate()
        {
            if(CanMove && _receiver.ControlsEnabled)
                Move();
            else
            {
                _playerOneMovement = Vector2.zero;
                _playerTwoMovement = Vector2.zero;
            }
        }
        
        private void LateUpdate()
        {
            if (HorizontalDirection < -0.1f)
                FacingRight = false;
            else if(HorizontalDirection > 0.1f)
                FacingRight = true;
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
            jumpEvents.finished += CheckForDoubleJump;
        }

        private void SubscribeDashActions()
        {
            var bDashAction = _mapper.ActionMapper.TryGetValue("Dash", out var dashEvents);
            Assert.IsTrue(bDashAction, "Dash input action not found in $InputMapper");

            dashEvents.checker += DashChecker;
            dashEvents.ok += Dash;
        }

        private void SubscribeMovementActions() => _mapper.onMove += UpdateMovement;


        //-----------
        // MOVEMENT
        //-----------
        private void UpdateMovement(int mindId, Vector2 inputVector)
        {
            if (mindId == 0)
                _playerOneMovement = inputVector;
            else if (mindId == 1)
                _playerTwoMovement = inputVector;
        }

        private void Move()
        {
            HorizontalDirection = ((_playerOneMovement + _playerTwoMovement) / 2f).x;
            
            bodyAnimator.SetFloat(movementAnimationVar, Mathf.Abs(HorizontalDirection));
            bodyAnimator.SetBool(runningAnimationVar, Mathf.Abs(HorizontalDirection) > 0.5f);
            
            transform.Translate(Vector2.right * (HorizontalDirection * speed.Value * Time.deltaTime));
        }

        //---------
        // JUMPS
        //---------
        private bool PerformNormalJump(float reactionTime)
        {
            var canJump = _jumpCount < 2;

            if (canJump)
            {
                _airTime = 0;
                _jumping = true;
                _jumpCompleted = false;
                _jumpCount++;
                
                if(_coyoteCheck)
                    ResetCoyoteTime();
                
                bodyAnimator.SetTrigger(jumpAnimationVar);

                _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
                _myRigidBody.AddForce(Vector2.up * jumpForceAlone.Value, ForceMode2D.Impulse);
            }
            
            return canJump;
        }

        private void PerformSuperJump(float elapsedTime)
        {
            _jumpCompleted = true;
            _airTime = 0;
            //_myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
            _myRigidBody.AddForce(Vector2.up * jumpForceCombined.Value, ForceMode2D.Impulse);
        }

        private void CheckForDoubleJump() => _canDoubleJump = (!IsGrounded && _jumpCompleted);

        private void OnPlayerTouchesGround()
        {
            if (_jumping && !_jumpCompleted)
                _mapper.ActionMapper["Jump"].finished?.Invoke();

            _airTime = 0;
            _jumping = false;
            _jumpCompleted = false;
            _canDoubleJump = false;
            _jumpCount = 0;
            _canDash = true;
        }
    
        //---------
        // DASHES
        //---------
        private void Dash(float elapsedTime)
        {
            var dd = dashDistance.Value;
            var movDistance = elapsedTime < dashSuccessMaxTime.Value ? dd : (1 - elapsedTime) * dd;
            var direction = _facingRight ? 1 : -1;
            var destiny = transform.position + (Vector3.right * movDistance * direction);
            
            CanMove = false;
            _airTime = 0;
            bodyAnimator.SetTrigger(dashAnimationVar);
            
            // Si queremos que no se puedan encadenar dash y saltos descomentar esto
            _jumpCount++;

            if (!_isGrounded)
                _canDash = false;

            hitChecker.enabled = false;
            _myRigidBody.DOMove(destiny, dashTime.Value).SetEase(Ease.OutFlash).onComplete += () =>
            {
                hitChecker.enabled = true;
                CanMove = true;
                bodyAnimator.SetBool(dashAnimationVar, false);

            };
        }

        private bool DashChecker(float t) => _canDash;
        
        //----------------
        // CHECKPOINT
        //----------------
        public void BackToCheckpoint()
        {
            _myRigidBody.velocity = Vector2.zero;
            transform.position = _lastFullyOnGround;
            StartCoroutine(WaitForControl(0.5f));
        }

        private IEnumerator WaitForControl(float time)
        {
            _receiver.ControlsEnabled = false;
            yield return new WaitForSeconds(time);
            _receiver.ControlsEnabled = true;
        }

        private void UpdateCheckpointPos()
        {
            var checkers = 0;
            foreach (var checker in groundCheckers)
            {
                if (Physics2D.Raycast(checker.position, Vector2.down, 0.2f, whatIsNotCheckpoint))
                    checkers++;
            }

            if (checkers < groundCheckers.Length)
                _lastFullyOnGround = transform.position;
        }  

        //----------------
        // GROUND CHECK
        //----------------
        private bool CheckGrounded()
        {
            var checkers = 0;

            foreach (var checker in groundCheckers)
            {
                if (Physics2D.Raycast(checker.position, Vector2.down, 0.2f, whatIsGround))
                    checkers++;
            }
            
            // For the CheckpointCheck
            if (checkers == groundCheckers.Length)
                UpdateCheckpointPos();

            bodyAnimator.SetBool(groundedAnimationVar, checkers > 0);
            
            return checkers > 0;
        }

        //--------------
        // COYOTE TIME
        //--------------
        private IEnumerator CoyoteTime(float t)
        {
            // If jumping don't activate Coyote Time
            if (_myRigidBody.velocity.y > 0) yield break;

            _coyoteCheck = true;
            
            var elapsedTime = 0f;

            while (elapsedTime < t)
            {
                _isGrounded = true;
                
                elapsedTime += Time.deltaTime;
                
                if(CheckGrounded()) ResetCoyoteTime();
                
                yield return new WaitForEndOfFrame();
            }
            
            ResetCoyoteTime();
        }
        
        private void ResetCoyoteTime() => _coyoteCheck = false;
        
        // UTILS
        public void DisableInput(float time) => StartCoroutine(DisableInputCoroutine(time));
        
        private IEnumerator DisableInputCoroutine(float time)
        {
            _receiver.ControlsEnabled = false;
            CanMove = false;
            yield return new WaitForSeconds(time);
            _receiver.ControlsEnabled = true;
            CanMove = true;
        }
        
        // GIZMOS...
        private void OnDrawGizmos()
        {
            foreach (var t in groundCheckers)
            {
                var position = t.position;
                
                Gizmos.DrawLine(position, position + Vector3.down * 0.2f);
            }
            Gizmos.DrawLine(transform.position, transform.position + bodyPart.right * 0.2f);
            Gizmos.color = new Color(0.4f, 0.84f, 1f, 0.1f);
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}