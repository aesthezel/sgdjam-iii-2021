using UnityEngine;

namespace Code.Hero
{
    public class PlayerReceiver : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [Header("Stats")] 
        [SerializeField] private int _lifes;
        [SerializeField] private float _speed;

        public int Lifes
        {
            get => _lifes;
        }
        
        public float HorizontalDirection { get; set; }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity += Vector2.right * (HorizontalDirection * _speed * Time.deltaTime);
        }
        
        // TODO: implementar eventos con Actions o UnityEvents que transmitan las condiciones del jugador
    }
}