using System;
using Code.Hero.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Hero
{
    public class Mind
    {
        [SerializeField] private int _id;
        public int ID
        {
            get => _id;
        }
        
        [SerializeField] private PlayerInput _playerInput;

        private Vector2 _suggestedDirection;

        public Action<IInteraction> OnInteractionPerform;

        public void PerformMoveViaInput(InputAction.CallbackContext context) => 
            _suggestedDirection = context.ReadValue<Vector2>();
    }
}