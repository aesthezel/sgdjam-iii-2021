using UnityEngine;

namespace Code.Utils.Environment
{
    public class FallingStalactite: MonoBehaviour
    {
        [SerializeField] private float distance;
        private bool _falling;
        
        private void Update()
        {
            var hits = Physics2D.RaycastAll(transform.position, Vector2.down, distance);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _falling = true;
                    GetComponent<Rigidbody2D>().gravityScale = 2.5f;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_falling)
                Destroy(this.gameObject);
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.DrawLine(position, position + Vector3.down * distance);
        }
    }
}