using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Testing
{
    public class MovePlayerTest : MonoBehaviour
    {
        [SerializeField] private InputAction MoveBinding;
        [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private AnimationCurve decelerationCurve;
        [SerializeField] private float moveVelocity;
        [SerializeField] private LayerMask whatIsWall;

        // Movement
        private Vector2 _currentVelocity;
        private Coroutine _moveCoroutine;
        private Action<float> _onDirectionChange;

        public Vector2 CurrentVelocity { get; set; }


        // Input
        private Vector2 _playerInput;
        public Vector2 PlayerInput
        {
            get => _playerInput;
            
            private set
            {
                if((int)_playerInput.x != (int)value.x)
                    _onDirectionChange?.Invoke(value.x);

                _playerInput = value;
            }
        }
        
        // Facing
        private float _facingDirection;
        private FaceCollissions _faceCollisions;

        private struct FaceCollissions
        {
            public bool leftCollision;
            public bool rightCollision;

            public void Reset() => leftCollision = rightCollision = false;
        }
        
        
        private void Awake()
        {
            MoveBinding.Enable();
            _facingDirection = Mathf.Sign(transform.localScale.x);
            _onDirectionChange += FacingDirectionCompute;
            _onDirectionChange += ComputeMovementAcceleration;
            MoveBinding.performed += (context) => GetInput(context);
            MoveBinding.canceled += (context) => GetInput(context);
        }

        private void Update()
        {
            // Test frontal collisions
            var origin = transform.position;
            var direction = Vector2.right * _facingDirection;
            var rayLength = 0.55f;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, whatIsWall);
            
            Debug.DrawLine(transform.position, transform.position + Vector3.right * 0.55f * _facingDirection);
            
            _faceCollisions.Reset();
            
            if (hit)
            {
                _faceCollisions.leftCollision = (int)_facingDirection == -1;
                _faceCollisions.rightCollision = (int)_facingDirection == 1;
            }

            _currentVelocity.y = Mathf.Clamp(_currentVelocity.y, -10, float.MaxValue);
            Move(_currentVelocity * Time.deltaTime);
        }

        
        // -------------
        // Input
        // -------------
        public void GetInput(InputAction.CallbackContext context) => PlayerInput = context.ReadValue<Vector2>();

        
        // -------------
        // Facing
        // -------------
        private void FacingDirectionCompute(float value)
        {
            if(value != 0) 
                _facingDirection = value > 0 ? 1 : -1;
            transform.localScale = new Vector3(_facingDirection, 1, 1);   
        }


        // -------------
        // Movement
        // -------------
        private void Move(Vector2 delta)
        {
            if (_faceCollisions.leftCollision || _faceCollisions.rightCollision)
                delta.x = 0f;
            transform.Translate(delta);
        }
        
        private void ComputeMovementAcceleration(float value)
        {
            if(_moveCoroutine != null) StopCoroutine(_moveCoroutine);

            if (_playerInput.x == 0 && Mathf.Abs(value) > 0)
                _moveCoroutine = StartCoroutine(AcceleratedMovement(accelerationCurve));
            
            else if (Mathf.Abs(_playerInput.x) > 0 && value == 0)
                _moveCoroutine = StartCoroutine(AcceleratedMovement(decelerationCurve));        
        }

        private IEnumerator DirectionChange()
        {
            _moveCoroutine = StartCoroutine(AcceleratedMovement(decelerationCurve));
            yield return new WaitUntil(() => _moveCoroutine == null);
            _moveCoroutine = StartCoroutine(AcceleratedMovement(accelerationCurve));
        }
        
        private IEnumerator AcceleratedMovement(AnimationCurve curve)
        {
            var keys = curve.keys;
            var accelerationTime = keys[keys.Length - 1].time - keys[0].time;
            var elapsedTime = 0f;

            if (_faceCollisions.leftCollision || _faceCollisions.rightCollision)
                elapsedTime = accelerationTime;
            
            while (elapsedTime < accelerationTime)
            {
                elapsedTime += Time.deltaTime;
                _currentVelocity.x = curve.Evaluate(elapsedTime) * moveVelocity * _facingDirection;
                yield return new WaitForEndOfFrame();
            }
            
            _currentVelocity.x = moveVelocity * curve.Evaluate(accelerationTime) * _facingDirection;
        }
    }
}