using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Code.Hero
{
    public class PlayerReceiver : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        public float HorizontalDirection { get; set; }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity += new Vector2(HorizontalDirection, 0f) * _speed * Time.deltaTime;
        }
    }
}