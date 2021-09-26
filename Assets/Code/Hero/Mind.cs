using Code.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Code.Hero
{
    public class Mind : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        private PlayerReceiver _playerReceiver;
        private Rigidbody2D _rigidbody2D;
        private ShowButtonsOnUI buttonController;
        
        [ReadOnly] private int _inputIndex;
        
        public int InputIndex
        {
            get => _inputIndex;
        }
        
        

        private void Awake()
        {
            // TODO: In a future we could stop using FindWithTag with a Service Locator
            _playerReceiver = GameObject.FindWithTag("Player").GetComponent<PlayerReceiver>();
            buttonController = GameObject.FindWithTag("ButtonChangeUI").GetComponent<ShowButtonsOnUI>();
            _inputIndex = playerInput.playerIndex;
        }

        public void MovementInput(InputAction.CallbackContext context)
        {
            if (_playerReceiver != null)
            {
                var movement = context.ReadValue<Vector2>();
                _playerReceiver.MovementInput(_inputIndex, movement);
                buttonController.ChangeImageMovement(_inputIndex, context.action.name, movement);
            }
        }

        public void PerformInput(InputAction.CallbackContext context)
        {
            if(_playerReceiver == null || !context.performed) return;

            var actionName = context.action.name;
            _playerReceiver.InputActionPerformed(_inputIndex, actionName);
            buttonController.ChangeImage(_inputIndex, actionName);
        }
    }
}