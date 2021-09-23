using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

namespace Code.Hero
{
    public class PlayerController : MonoBehaviour
    {
        [Header("--- Player Stats ---")]
        [SerializeField] private float _speed;
        [SerializeField] private int lifes;
        
        [Header("-- Jumping --")]
        [SerializeField] private LayerMask whatIsGround;
        [Space, SerializeField] 
        private Transform[] groundCheckers;
        
        // Components
        private Rigidbody2D _myRigidBody;
        private InputMapper _mapper;
        // Jumping
        private bool _coyoteCheck;
        private bool _isGrounded;
        private bool _jumping;
        private bool _jumpCompleted;
        
        // Movement
        private Vector2 _playerOneMovement;
        private Vector2 _playerTwoMovement;
        
        public bool IsGrounded
        {
            get => _isGrounded;
            private set
            {
                // From air to ground
                if (!_isGrounded && value && _jumping && !_jumpCompleted)
                {
                    _jumping = false;
                    _mapper.ActionMapper["Jump"].failed?.Invoke();
                }

                // From ground to air
                if (_isGrounded && !value && !_coyoteCheck)
                    StartCoroutine(CoyoteTime(0.15f));
                
                _isGrounded = value;
            }
        }
        
        public float HorizontalDirection { get; private set; }
        public int Lifes => lifes;

        
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
            CheckGrounded();
        }

        private void FixedUpdate()
        {
            Move();
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
            _myRigidBody.velocity += Vector2.right * (HorizontalDirection * _speed * Time.deltaTime);
        }

        //---------
        // JUMPS
        //---------
        private bool PerformNormalJump(float reactionTime)
        {
            if (IsGrounded)
            {
                _jumping = true;
                _jumpCompleted = false;
                _coyoteCheck = false;
                _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
                _myRigidBody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            }
            
            return IsGrounded;
        }

        private void PerformSuperJump(float elapsedTime)
        {
            _jumpCompleted = true;
            _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
            _myRigidBody.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        }
        
        
        //---------
        // DASHES
        //---------
        private void Dash(float elapsedTIme)
        {
            //TODO: Cuidado (1 - ...) asume que el valor de espera a player 2 sera de 1 segundo
            var movDistance = (1 - elapsedTIme) * 3f;
            transform.DOMoveX(transform.position.x + movDistance, 0.5f).SetEase(Ease.OutQuad);
        }
        
        
        //----------------
        // GROUND CHECK
        //----------------
        private void CheckGrounded()
        {
            var checkers = 0;

            foreach (var checker in groundCheckers)
            {
                if (Physics2D.Raycast(checker.position, Vector2.down, 0.2f, whatIsGround))
                    checkers++;
            }

            IsGrounded = checkers > 0 ? true : false;
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
                IsGrounded = true;
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

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