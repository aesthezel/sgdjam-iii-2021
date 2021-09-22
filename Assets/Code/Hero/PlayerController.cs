using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Hero
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsGround;

        [SerializeField, Header("Ground Checkers")]
        private Transform[] groundCheckers;
        
        private Rigidbody2D _myRigidBody;
        private InputMapper _mapper;
        
        private bool _isGrounded;
        private Coroutine _coyoteCoroutine;
        private bool coyoteCheck;
        
        public bool IsGrounded
        {
            get => _isGrounded;
            private set
            {
                // From air to ground
                if(!_isGrounded && value)
                    _mapper.ActionMapper["Jump"].failed?.Invoke();
                
                // From ground to air
                if (_isGrounded && !value && !coyoteCheck)
                    StartCoroutine(CoyoteTime(0.15f));
                
                _isGrounded = value;
            }
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
        }

        private void Update()
        {
            CheckGrounded();
        }


        //---------------------------------
        // SUBSCRIPTIONS TO INPUT ACTIONS
        //---------------------------------
        private void SubscribeJumpActions()
        {
            var bJumAction = _mapper.ActionMapper.TryGetValue("Jump", out var jumpEvents);
            Assert.IsTrue(bJumAction, "$Jump input action not found in $InputMapper");

            jumpEvents.start += PerformNormalJump;
            jumpEvents.ok += PerformSuperJump;
            // TODO: No llamar al salto cancelado si realmente no ha saltado
            jumpEvents.failed += () => Debug.Log("SALTO CANCELADO!");
        }
        
        //---------
        // JUMPS
        //---------
        private void PerformNormalJump(float reactionTime)
        {
            if (IsGrounded)
            {
                coyoteCheck = false;
                _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
                _myRigidBody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            }
        }

        private void PerformSuperJump(float elapsedTime)
        {
            _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
            _myRigidBody.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
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

            coyoteCheck = true;
            
            var elapsedTime = 0f;

            while (elapsedTime < t)
            {
                IsGrounded = true;
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            // TODO: Eliminar
            GetComponentInChildren<SpriteRenderer>().color = Color.green;
            IsGrounded = false;
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