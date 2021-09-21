using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Hero
{
    public enum InputState
    {
        None,
        First,
        Second
    }

    public class InputReceiver : MonoBehaviour
    {
        private InputState _inputState = InputState.None;
        
        private PlayerInput firstMind;
        private PlayerInput secondMind;

        private Vector2 firstMovement;
        private Vector2 secondMovement;

        public Vector2 FirstMovement
        {
            get => firstMovement;

            set
            {
                if(value.x > 0f || value.y > 0f)
                    StartCoroutine(FirstMindWaitingForSecondMind());
                
                firstMovement = value;
            }
        }

        public Vector2 SecondMovement
        {
            get => secondMovement;

            set
            {
                StartCoroutine(FirstMindWaitingForSecondMind());
                secondMovement = value;
            }
        }

        private bool firstMindCorrect;
        private bool secondMindCorrect;

        public void MovementByFirstMind(InputAction.CallbackContext context) => 
            FirstMovement = context.ReadValue<Vector2>();

        public void MovementBySecondFirstMind(InputAction.CallbackContext context) =>
            SecondMovement = context.ReadValue<Vector2>();

        // Update is called once per frame
        void FixedUpdate()
        {
        
        }

        public void Movement()
        {
        
        }
    
        // TODO: usar funciones asincronas en una clase a parte para perpetuar el tiempo de espera entre input.

        public IEnumerator FirstMindWaitingForSecondMind()
        {
            if (_inputState == InputState.None && _inputState != InputState.First)
            {
                float count = 0f;

                while (count < 3f)
                {
                    count++;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}