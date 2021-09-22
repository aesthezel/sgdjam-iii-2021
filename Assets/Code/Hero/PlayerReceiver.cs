using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Code.Hero
{
    public class PlayerReceiver : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [Header("Stats")] 
        [SerializeField] private int _lifes;
        [SerializeField] private float _speed;

        public int Lifes => _lifes;
        
        // PLAYER INPUT EVENTS

        public Action OnInputTimeUp;
        

        // On move sigue otro rollo
        public Action<float, float> OnMove;
        
        // Movement
        private Vector2 _playerOneMovement;
        private Vector2 _playerTwoMovement;
        
        // Jumping
        private string _requiredActionToComplete;
        private int _eventStartedrPlayer;
        public Action<float> OnJump;    // Debe recibir el tiempo que se ha tardado en rellenar

        
        // For all buttons
        private Coroutine _waitingCoroutine;
        
        // Properties
        public float HorizontalDirection { get; set; }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        // TODO: Los movimientos por fisicas son mas inexactos (tarda en parar) y requerimos rapidez y precision
        private void FixedUpdate()
        {
            var playerVelocity = (_playerOneMovement + _playerTwoMovement) / 2f;
            HorizontalDirection = playerVelocity.x;
            _rigidbody2D.velocity += Vector2.right * (HorizontalDirection * _speed * Time.deltaTime);
        }
        
        public void MovementInput(int mindId, Vector2 velocity)
        {
            if (mindId == 0)
                _playerOneMovement = velocity;
            else if (mindId == 1)
                _playerTwoMovement = velocity;
        }

        // TODO: Tal vez podria generalizarse a otros inputs facilmente
        public void JumpInputPressed(int mindId, string actionName)
        {
            Debug.Log($"{actionName} performed");
            
            if (string.IsNullOrEmpty(_requiredActionToComplete))
            {
                _requiredActionToComplete = actionName;
                _eventStartedrPlayer = mindId;
                
                if(_waitingCoroutine != null)
                    StopCoroutine(_waitingCoroutine);
                
                _waitingCoroutine = StartCoroutine(WaitFotOtherMind(1f));
                
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                _rigidbody2D.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            }
            
            else if(String.Equals(_requiredActionToComplete, actionName) 
                    && _eventStartedrPlayer != mindId)
            {
                Debug.Log("IN TIME!");
                
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                _rigidbody2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                
                OnJump?.Invoke(0); // TODO: 0 seria que le han dado exactamente a la vez. Falta por hacer
            }
        }

        private IEnumerator WaitFotOtherMind(float time)
        {
            // TODO: Pensar en pasar el Mind, para desactivar el control del player durante el tiempo ralentizado
            var elapsedTime = 0f;

            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("TIME OUT!!");
            _requiredActionToComplete = default;
        }
    }
}