using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Code.Testing
{
    public class JumpTestPlayer : MonoBehaviour
    {
        public InputAction jumpButton;
        public LayerMask whatIsWall;
        public int GroundCheckers = 4;
        public LayerMask whatIsGround;
        public Vector2 velocity;
        public Vector2 position;
        public float gravity = -10;
        public float gravityModifier = 2f;
        private float currentGravityModifier = 2f;
        
        private Collider2D myCollider;
        private Vector2 bottomLeftPoint;
        private float rayDistance;
        public float jumpVel;
        private bool _grounded;

        public bool Grounded
        {
            get => _grounded;
            set
            {
                if (!_grounded && value)
                {
                    currentGravityModifier = gravityModifier;
                    
                    if(velocity.y < 0)
                        velocity.y = 0;
                }

                _grounded = value;
            }
        }


        private void Start()
        {
            GroundCheckers = Mathf.Clamp(GroundCheckers, 2, int.MaxValue);
            myCollider = GetComponent<Collider2D>();
        
            var bounds = myCollider.bounds;
            bottomLeftPoint = new Vector2(bounds.min.x, bounds.min.y);
            rayDistance = bounds.size.x / (GroundCheckers - 1);

            jumpButton.Enable();
            jumpButton.performed += (_) =>
            {
                velocity.y = jumpVel;
                //StartCoroutine(JumpAnim());
            };
        }

        private void LateUpdate()
        {
            UpdateRayPosition();
        
            CheckGrounded();

            if (!Grounded)
            {
                velocity.y += gravity * Time.deltaTime * currentGravityModifier;
                currentGravityModifier += Time.deltaTime;
            }

            // Check collisions on top
            if (!Grounded)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.55f, whatIsWall);
                Debug.DrawLine(transform.position, transform.position + Vector3.up * 0.55f, Color.red);
                if (hit && velocity.y > 0f)
                    velocity.y = 0;
            }

            Move(velocity * Time.deltaTime);
        }

        void UpdateRayPosition()
        {
            var bounds = myCollider.bounds;
            bottomLeftPoint = new Vector2(bounds.min.x, bounds.min.y);
        }
    
        private void CheckGrounded()
        {
            for (var i = 0; i < GroundCheckers; i++)
            {
                var origin = bottomLeftPoint + Vector2.right * rayDistance * i;
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.05f, whatIsGround);
            
                Debug.DrawLine(origin, origin + Vector2.down * 0.05f);
            
                if (hit)
                {
                    Grounded = true;
                    return;
                }
            }
        
            Grounded = false;
        }

        private void Move(Vector2 velocity) => transform.Translate(velocity);

        IEnumerator JumpAnim()
        {
            transform.localScale = new Vector3(1.03f, 0.98f, 1);
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(1.05f, 0.95f, 1);
            yield return new WaitForSeconds(0.1f);
            velocity.y = jumpVel;
            transform.localScale = new Vector3(0.95f, 1.05f, 1);
            yield return new WaitUntil(() => transform.position.y > 2);
            transform.localScale = new Vector3(1f, 1, 1);
        }
    }
}