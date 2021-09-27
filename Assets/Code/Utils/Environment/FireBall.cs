using System;
using UnityEngine;


namespace Code.Utils.Environment
{
    public class FireBall : MonoBehaviour
    {
        public Vector2 Direction { get; set; } = Vector2.right;
        public float speed = 150f;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = (Direction * speed) * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            gameObject.SetActive(false);
        }
    }
}