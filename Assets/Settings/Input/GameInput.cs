// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/GameInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Settings.Input
{
    public class @GameInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""FirstMind"",
            ""id"": ""5950bec6-71f0-4296-be1f-2c1972849bb5"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""e1f3e5d3-292d-4433-80cb-7d2c767acc46"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""ca02ac47-c994-46dd-9137-2e7ddff91063"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""159086e0-0dbe-4f1f-811b-75433b5d37f0"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""26bca5ec-90ff-4dd4-8181-36394df1e078"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""51e8d05c-069d-42ff-b179-247ec8bcb66f"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""68caf65d-bc16-4ec0-82b2-a76719195ec1"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""SecondMind"",
            ""id"": ""e30a5773-437a-4303-9469-d6db10ae5891"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""26a99bb4-bb06-4456-9914-907e123f88b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""32ceb85e-3186-403b-b982-9eead13f79c7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3dd50b07-d3e3-4fff-93a7-284e05d8fe34"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""994322fd-a24d-494a-80cb-d8585512b0d7"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d58d9376-ea0e-4468-a1b0-6c322ea842d9"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e991c511-63e0-415d-8972-0f04cc650f38"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // FirstMind
            m_FirstMind = asset.FindActionMap("FirstMind", throwIfNotFound: true);
            m_FirstMind_Movement = m_FirstMind.FindAction("Movement", throwIfNotFound: true);
            // SecondMind
            m_SecondMind = asset.FindActionMap("SecondMind", throwIfNotFound: true);
            m_SecondMind_Movement = m_SecondMind.FindAction("Movement", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // FirstMind
        private readonly InputActionMap m_FirstMind;
        private IFirstMindActions m_FirstMindActionsCallbackInterface;
        private readonly InputAction m_FirstMind_Movement;
        public struct FirstMindActions
        {
            private @GameInput m_Wrapper;
            public FirstMindActions(@GameInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_FirstMind_Movement;
            public InputActionMap Get() { return m_Wrapper.m_FirstMind; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(FirstMindActions set) { return set.Get(); }
            public void SetCallbacks(IFirstMindActions instance)
            {
                if (m_Wrapper.m_FirstMindActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_FirstMindActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_FirstMindActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_FirstMindActionsCallbackInterface.OnMovement;
                }
                m_Wrapper.m_FirstMindActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                }
            }
        }
        public FirstMindActions @FirstMind => new FirstMindActions(this);

        // SecondMind
        private readonly InputActionMap m_SecondMind;
        private ISecondMindActions m_SecondMindActionsCallbackInterface;
        private readonly InputAction m_SecondMind_Movement;
        public struct SecondMindActions
        {
            private @GameInput m_Wrapper;
            public SecondMindActions(@GameInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_SecondMind_Movement;
            public InputActionMap Get() { return m_Wrapper.m_SecondMind; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(SecondMindActions set) { return set.Get(); }
            public void SetCallbacks(ISecondMindActions instance)
            {
                if (m_Wrapper.m_SecondMindActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_SecondMindActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_SecondMindActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_SecondMindActionsCallbackInterface.OnMovement;
                }
                m_Wrapper.m_SecondMindActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                }
            }
        }
        public SecondMindActions @SecondMind => new SecondMindActions(this);
        public interface IFirstMindActions
        {
            void OnMovement(InputAction.CallbackContext context);
        }
        public interface ISecondMindActions
        {
            void OnMovement(InputAction.CallbackContext context);
        }
    }
}
