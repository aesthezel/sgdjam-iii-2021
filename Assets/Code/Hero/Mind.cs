using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Hero
{
    public class Mind : MonoBehaviour
    {
        [SerializeField] PlayerReceiver _playerReceiver;
        
        private Rigidbody2D _rigidbody2D;

        [ReadOnly] private int _inputIndex;
        public int InputIndex
        {
            get => _inputIndex;
        }
        
        [SerializeField] private PlayerInput _playerInput;

        private void Awake()
        {
            _playerReceiver = GameObject.FindWithTag("Player").GetComponent<PlayerReceiver>();
            _inputIndex = _playerInput.playerIndex;
        }

        public void PerformMoveViaInput(InputAction.CallbackContext context)
        {
            var movement = context.ReadValue<Vector2>();
            Debug.Log(movement);
            
            _playerReceiver.HorizontalDirection = movement.x;
        }
    }
}