using UnityEngine;


namespace Code.Utils.Environment
{
    public class FireBall : MonoBehaviour
    {
        public Vector2 direction { get; set; } = Vector2.right;

        private void Update()
        {
            transform.Translate(direction * 2.5f * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            this.gameObject.SetActive(false);
        }
    }
}