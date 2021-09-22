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

        public void MovementInput(InputAction.CallbackContext context)
        {
            if(_playerReceiver != null)
                _playerReceiver.MovementInput(_inputIndex, context.ReadValue<Vector2>());
        }

        public void PerformInput(InputAction.CallbackContext context)
        {
            if(_playerReceiver == null) return;
            
            if (context.performed)
                _playerReceiver.InputActionPerformed(_inputIndex, context.action.name);
        }
    }
}