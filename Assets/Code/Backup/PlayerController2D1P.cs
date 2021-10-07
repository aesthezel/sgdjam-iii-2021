using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

namespace Code.Hero
{
    
    public class PlayerController2D1P : MonoBehaviour
    {
        //TODO: Esto desaparecera
        [SerializeField] private InputAction jumpButton;
        
        [BoxGroup("Movement", centerLabel: true)]
        [SerializeField] private AnimationCurve accelerationCurve;
        [BoxGroup("Movement")]
        [SerializeField] private AnimationCurve decelerationCurve;
        [BoxGroup("Movement")]
        [SerializeField] private float moveVelocity;
        [BoxGroup("Movement")]
        [SerializeField] private LayerMask whatIsWall;
        
        [BoxGroup("Physics", centerLabel: true)]
        [SerializeField] private float skinWidth = 0.0015f;
        [BoxGroup("Physics")]
        [SerializeField] private int groundCheckers = 4;
        [BoxGroup("Physics")]
        [SerializeField] private int faceCheckers = 4;
        
        [BoxGroup("Jumping", centerLabel: true)]
        [SerializeField] private float jumpVel;
        [BoxGroup("Jumping")]
        [SerializeField] private float gravity = -10;
        [BoxGroup("Jumping")]
        [SerializeField] private float gravityModifier = 2f;
        [BoxGroup("Jumping")]
        [SerializeField] private LayerMask whatIsGround;
        
        
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
        private Vector2 _playerInput;

        
        // -------------------
        // Properties
        // -------------------
        public Vector2 PlayerInput
        {
            get => _playerInput; 
            private set => SetPlayerInput(value);
        }

        public bool Grounded
        {
            get => _grounded; 
            private set => SetGrounded(value);
        }
        
        public PlayerCollisions PlayerCollisions => _playerCollisions;
        
        // properties' setters
        private void SetGrounded(bool value)
        {
            // If player touches the ground...
            if (!_grounded && value)
            {
                _currentGravityModifier = gravityModifier;
                _currentVelocity.y = _currentVelocity.y < 0 ? 0 : _currentVelocity.y;
            }

            _grounded = value;
        }

        private void SetPlayerInput(Vector2 value)
        {
            if((int)_playerInput.x != (int)value.x) _onDirectionChange?.Invoke(value.x);
            _playerInput = value;
        }
        
        
        // -------------------
        // UNITY METHODS
        // -------------------
        private void Start()
        {
            // At least we need 2 checkers for collision detection
            groundCheckers = Mathf.Clamp(groundCheckers, 2, int.MaxValue);
            faceCheckers = Mathf.Clamp(faceCheckers, 2, int.MaxValue);
            
            // Preferably to use BoxCollider2D
            _myCollider = GetComponent<Collider2D>();
            
            // Recalculate the facing direction when player changes direction
            _onDirectionChange += CalculateFacingDirection;
            _onDirectionChange += ComputeMovementAcceleration;
            
            // Calculate direction for the first time, since player hasn't made any direction change yet
            CalculateFacingDirection(Mathf.Sign(transform.localScale.x));
            
            // TODO: Cambiar a los controles bicefalos
            jumpButton.Enable();
            jumpButton.performed += (_) =>
            {
                _currentVelocity.y = jumpVel;
            };
        }

        private void Update()
        {
            UpdateRayOrigins();
            
            CheckGrounded();

            if (!Grounded)
            {
                CheckCeilCollision();
                ApplyGravity();
            }
            
            CheckFaceCollisions();
            
            // Move the amount computed by our physics
            Move(_currentVelocity * Time.deltaTime);
        }

        
        // -------------
        // Input
        // -------------
        public void GetInput(InputAction.CallbackContext context) => PlayerInput = context.ReadValue<Vector2>();

        
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
        // Movement
        // -------------
        private void Move(Vector2 delta)
        {
            // Clamp X if colliding with walls
            if (_playerCollisions.leftCollision || _playerCollisions.rightCollision)
                delta.x = 0f;

            // Don't let player break ceil or ground collisions 
            if (!Grounded)
            {
                ClampTopDistance(ref delta);
                ClampOnFalling(ref delta);
            }
            
            // Move clamped delta
            transform.Translate(delta);
        }
        
        private void ComputeMovementAcceleration(float value)
        {
            if(_moveCoroutine != null) StopCoroutine(_moveCoroutine);

            if (_playerInput.x == 0 && Mathf.Abs(value) > 0)
                _moveCoroutine = StartCoroutine(Accelerate());
            
            else if (Mathf.Abs(_playerInput.x) > 0 && value == 0)
                _moveCoroutine = StartCoroutine(Decelerate());        
        }
        
        private IEnumerator Accelerate()
        {
            var keys = accelerationCurve.keys;
            var accelerationTime = keys[keys.Length - 1].time - keys[0].time;
            var elapsedTime = 0f;

            while (elapsedTime < accelerationTime)
            {
                elapsedTime += Time.deltaTime;
                _currentVelocity.x = accelerationCurve.Evaluate(elapsedTime) * moveVelocity * _facingDirection;
                yield return new WaitForEndOfFrame();
            }
            
            _currentVelocity.x = moveVelocity * accelerationCurve.Evaluate(accelerationTime) * _facingDirection;
        }
        
        private IEnumerator Decelerate()
        {
            var keys = decelerationCurve.keys;
            var decelerationTime = keys[keys.Length - 1].time - keys[0].time;
            var elapsedTime = 0f;

            if (_playerCollisions.leftCollision || _playerCollisions.rightCollision) 
                yield break;
            
            while (elapsedTime < decelerationTime)
            {
                elapsedTime += Time.deltaTime;
                _currentVelocity.x = decelerationCurve.Evaluate(elapsedTime) * moveVelocity * _facingDirection;
                yield return new WaitForEndOfFrame();
            }
            
            _currentVelocity.x = moveVelocity * decelerationCurve.Evaluate(decelerationTime) * _facingDirection;
        }
        
        
        // --------------------------
        // CUSTOM PHYSICS
        // --------------------------
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
        }

        private void CheckFaceCollisions()
        {
            var faceOrigin = _facingDirection > 0 ? _origins.bottomRight : _origins.bottomLeft;
            var faceDirection = Vector2.right * _facingDirection;
            var faceHits = MultiRayEmiter(faceOrigin, faceDirection, 0.05f, whatIsWall);

            _playerCollisions.ResetFaceCollisions();
            
            if (faceHits.hasHit.Contains(true))
            {
                _playerCollisions.leftCollision = (int)_facingDirection == -1;
                _playerCollisions.rightCollision = (int)_facingDirection == 1;
            }
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
    }
}