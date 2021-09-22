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
        
        // TODO: Implementar una especie de coyote time
        private bool _isGrounded;
        public bool IsGrounded => _isGrounded;

        private void Awake()
        {
            _myRigidBody = GetComponent<Rigidbody2D>();
            _mapper = GetComponent<InputMapper>();
        }

        private void Start()
        {
            Debug.Log(_mapper.ActionMapper);
            var bJumAction = _mapper.ActionMapper.TryGetValue("Jump", out var jumpEvents);
            Assert.IsTrue(bJumAction, "$Jump input action not found in $InputMapper");

            jumpEvents.start += PerformNormalJump;
            jumpEvents.ok += PerformSuperJump;
        }


        private void Update()
        {
            CheckGrounded();
            
        }

        private void PerformNormalJump(float reactionTime)
        {
            if (_isGrounded)
            {
                _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
                _myRigidBody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            }
        }

        private void PerformSuperJump(float elapsedTime)
        {
            _myRigidBody.velocity = new Vector2(_myRigidBody.velocity.x, 0);
            _myRigidBody.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        }
        

        private void CheckGrounded()
        {
            var enabledCheckers = 0;
            var lastValue = _isGrounded;
            
            for (int i = 0; i < groundCheckers.Length; i++)
            {
                bool hit = Physics2D.Raycast(groundCheckers[i].position, Vector2.down, 0.2f);

                if (hit)
                    enabledCheckers += 1;
            }
            
            _isGrounded = enabledCheckers == groundCheckers.Length? true: false;
            

            if (!lastValue && _isGrounded)
                _mapper.ActionMapper["Jump"].failed?.Invoke();
            
            Debug.Log(_isGrounded);
        }
        
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