using System.Collections;
using Code.Data;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

namespace Code.Hero
{
    public class PlayerController : MonoBehaviour
    {
        [Header("--- Player Stats ---")]
        [SerializeField] private FloatData _speed;
        [SerializeField] private IntData lifes;

        [Header("-- Jumping --")]
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private FloatData coyoteTimeValue;
        [Space]
        [SerializeField] private Transform[] groundCheckers;

        [Header("-- Looking --")] 
        [SerializeField] private Transform bodyPart;
        
        // Components
        private Rigidbody2D _myRigidBody;
        private InputMapper _mapper;

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

        
        private bool _facingRight;
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

        public Vector3 LastFullyOnGround
        {
            get;
            private set;
        }
        
        
        //----------------
        // UNITY METHODS
        //----------------
        private void Awake()
        {
            _myRigidBody = GetComponent<Rigidbody2D>();
            _mapper = GetComponent<InputMapper>();
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
        }

        private void FixedUpdate()
        {
            Move();

            if (!_isGrounded)
            {
                _airTime += Time.fixedDeltaTime;
                _myRigidBody.velocity += Vector2.down * _airTime;
            }
        }
        
        private void LateUpdate()
        {
            FacingRight = !(HorizontalDirection < -0.1f);
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

            dashEvents.ok += Dash;
        }

        private void SubscribeMovementActions() => _mapper.OnMove += UpdateMovement;


        //-----------
        // MOVEMENT
        //-----------
        private void UpdateMovement(int mindId, Vector2 velocity)
        {
            if (mindId == 0)
                _playerOneMovement = velocity;
            else if (mindId == 1)
                _playerTwoMovement = velocity;
        }

        private void Move()
        {
            HorizontalDirection = ((_playerOneMovement + _playerTwoMovement) / 2f).x;
            transform.Translate(Vector2.right * (HorizontalDirection * _speed.Value * Time.deltaTime));
            //_myRigidBody.velocity += Vector2.right * (HorizontalDirection * _speed.Value * Time.deltaTime);
        }

        //---------
        // JUMPS
        //---------
        private bool PerformNormalJump(float reactionTime)
        {
            var canJump = (IsGrounded || _canDoubleJump) && _jumpCount < 2 ;

            if (canJump)
            {
                _airTime = 0;
                _jumping = true;
                _jumpCompleted = false;
                _jumpCount++;
                
                if(_coyoteCheck)
                    ResetCoyoteTime();

                _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
                _myRigidBody.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            }
            
            return canJump;
        }

        private void PerformSuperJump(float elapsedTime)
        {
            _jumpCompleted = true;
            _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
            _myRigidBody.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
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
        }
    
        //---------
        // DASHES
        //---------
        private void Dash(float elapsedTIme)
        {
            //TODO: Cuidado (1 - ...) asume que el valor de espera a player 2 sera de 1 segundo
            var movDistance = (1 - elapsedTIme) * 3f;
            var direction = _facingRight ? 1 : -1;
            transform.DOMoveX(transform.position.x + (movDistance * direction), 0.5f).SetEase(Ease.OutQuad);
        }

        //----------------
        // CHECKPOINT
        //----------------
        public void BackToCheckpoint() => transform.position = LastFullyOnGround;

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
                LastFullyOnGround = transform.position;

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

        // GIZMOS...
        private void OnDrawGizmos()
        {
            foreach (var t in groundCheckers)
            {
                var position = t.position;
                
                Gizmos.DrawLine(position, position + Vector3.down * 0.2f);
            }
        }
    }
}