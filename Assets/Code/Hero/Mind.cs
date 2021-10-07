using System;
using Code.Services;
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
        private ShowButtonsOnUI _buttonController;
        
        [ReadOnly] private int _inputIndex;
        
        public int InputIndex
        {
            get => _inputIndex;
        }

        private void Awake()
        {
            _inputIndex = playerInput.playerIndex;
        }

        private void Start()
        {
            _buttonController = ServiceLocator.Instance.ObtainService<UIService>().ButtonDisplayer;
            _playerReceiver = ServiceLocator.Instance.ObtainService<PlayerService>().Receiver;
        }

        public void MovementInput(InputAction.CallbackContext context)
        {
            if (_playerReceiver != null)
            {
                var movement = context.ReadValue<Vector2>();
                _playerReceiver.MovementInput(_inputIndex, movement);
                _buttonController.ChangeImageMovement(_inputIndex, context.action.name, movement);
            }
        }

        public void PerformInput(InputAction.CallbackContext context)
        {
            if(_playerReceiver == null || !context.performed) return;

            var actionName = context.action.name;
            _playerReceiver.InputActionPerformed(_inputIndex, actionName);
            _buttonController.ChangeImage(_inputIndex, actionName);
        }
    }
}