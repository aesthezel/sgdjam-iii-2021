using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;
using UnityEditor.Animations;

namespace Code.Hero
{
    [RequireComponent(typeof(InputMapper))]
    public class PlayerReceiver : MonoBehaviour
    {
        [Header("Stats")] 
        [SerializeField] private int _lifes;
        [SerializeField] private float _speed;
        
        
        public int Lifes => _lifes;

        // On move sigue otro rollo
        public Action<float, float> OnMove;

        // Movement
        private Vector2 _playerOneMovement;
        private Vector2 _playerTwoMovement;
        private Rigidbody2D _rigidbody2D;

        // For buttons that require waiting
        private string _requiredActionToComplete;
        private Coroutine _waitingCoroutine;
        private float _timeUntilComplete;
        private int _firstMindId;

        // Input Mapper
        private InputMapper _mapper;
        
        // Properties
        public float HorizontalDirection { get; set; }

        private Tween _cameraTween;
        
        // --------------
        // UNITY METHODS
        // --------------

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _mapper = GetComponent<InputMapper>();
        }

        private void Start()
        {
            // Por cada input asignamos el error a reiniciar esta clase
            // Esto esta bien y por ahora es super necesario
            var keys = _mapper.ActionMapper.Keys;
            foreach (var key in keys)
            {
                if (_mapper.ActionMapper.TryGetValue(key, out var action)) 
                    action.failed += ResetInputEventSystem;
            }
            
            var asuccess = _mapper.ActionMapper.TryGetValue("Dash", out var dashEvents);
            Assert.IsTrue(asuccess, "Dash error");
            
            // Testing Dash
            dashEvents.start += (t) =>
            {
                _cameraTween = Camera.main.DOOrthoSize(5.5f, 0.2f);
                GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                Debug.Log("Dash Empieza");
            };
            
            dashEvents.ok += t =>
            {
                _cameraTween.Kill();
                _cameraTween = Camera.main.DOOrthoSize(6, 0.05f);
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
                var movDistance = (1 - t) * 3f;
                Debug.Log($"Elapsed time {t}, movement: {movDistance}");
                transform.DOMoveX(transform.position.x + movDistance, 0.5f);
            };
            
            dashEvents.failed += ( ) =>
            {
                _cameraTween.Kill();
                _cameraTween = Camera.main.DOOrthoSize(6, 0.2f);
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
                Debug.Log("Dash Fallado");
            };
        }

        private void Update()
        {
            if (_waitingCoroutine != null)
                _timeUntilComplete += Time.deltaTime;
        }

        // TODO: Los movimientos por fisicas son mas inexactos (tarda en parar) y requerimos rapidez y precision
        private void FixedUpdate()
        {
            var playerVelocity = (_playerOneMovement + _playerTwoMovement) / 2f;
            HorizontalDirection = playerVelocity.x;
            _rigidbody2D.velocity += Vector2.right * (HorizontalDirection * _speed * Time.deltaTime);
        }
        
        // --------------
        // INPUT METHODS
        // --------------
        public void MovementInput(int mindId, Vector2 velocity)
        {
            if (mindId == 0)
                _playerOneMovement = velocity;
            else if (mindId == 1)
                _playerTwoMovement = velocity;
        }
        
        public void InputActionPerformed(int mindId, string actionName)
        {
            //Debug.Log($"{actionName} performed");

            if (string.IsNullOrEmpty(_requiredActionToComplete))
                InputEventStart(mindId, actionName, 1f);
            
            else if(_firstMindId != mindId)
                InputEventFinish(mindId, actionName);
        }
        
        private void InputEventStart(int mindId, string actionName, float waitTime)
        {
            _requiredActionToComplete = actionName;
            _firstMindId = mindId;
            
            var success = _mapper.ActionMapper.TryGetValue(actionName, out var actions);
            
            Assert.IsTrue(success, $"InputAction name {actionName} not found in <ActionMapper>");

            // Event Starts
            actions.start?.Invoke(waitTime);
            _waitingCoroutine = StartCoroutine(WaitForOtherMind(waitTime, actions.failed));
        }

        private void InputEventFinish(int mindId, string actionName)
        {
            var success = _mapper.ActionMapper.TryGetValue(actionName, out var actions);

            Assert.IsTrue(success, $"InputAction name {actionName} not found in <ActionMapper>");

            if (String.Equals(_requiredActionToComplete, actionName))
            {
                // Event completed correctly
                Debug.Log("IN TIME!");
                actions.ok?.Invoke(_timeUntilComplete);  
            }

            else
                // Event failed
                actions.failed?.Invoke();
        }
        
        
        private IEnumerator WaitForOtherMind(float time, Action actionOnFail)
        {
            // TODO: Pensar en pasar el Mind, para desactivar el control del player durante el tiempo ralentizado
            var elapsedTime = 0f;

            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            Debug.Log("TIME OUT!!");
            actionOnFail?.Invoke();
        }

        private void ResetInputEventSystem()
        {
            if(_waitingCoroutine != null)
                StopCoroutine(_waitingCoroutine);
            
            _requiredActionToComplete = default;
            _firstMindId = default;
            _timeUntilComplete = 0;
            _waitingCoroutine = null;
        }
    }
}