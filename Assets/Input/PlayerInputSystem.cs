//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/PlayerInputSystem.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputSystem: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputSystem"",
    ""maps"": [
        {
            ""name"": ""Control"",
            ""id"": ""b3b5095d-7ea5-4d57-a8f5-a05cb049d3b6"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""2f4b3525-0563-4eca-bd1b-d37d183bd84e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""c68f48c3-f2de-4a7c-afd5-96b98a0684a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""View"",
                    ""type"": ""Value"",
                    ""id"": ""6b5d7bce-750f-4b82-97cf-88df2d8e849e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""38c8c7ba-487d-4b6e-9207-0120301fd68c"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ViewDirection"",
                    ""type"": ""Button"",
                    ""id"": ""47b772c0-df47-4c95-9826-5f97e7310e37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""92e71934-113a-4a67-b43b-1ad1b35030c6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""cbf2ccb4-b635-4a28-b1eb-ed516de7c307"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""9824aab7-a128-4939-bbe6-b8f2be579bfd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""08aebc59-01cd-4615-945b-d5b77fb8916e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""204597b9-68f4-4e2c-8877-dda8c548ea2f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5dda9cdb-0570-4ba1-a9b8-1406fb27fed7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e757c5fc-5e05-4dd4-b8d7-bb52e0d8522e"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bde0c27-6727-4d1d-a11d-c377c5196104"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df07bdba-d9f0-4bbf-8d57-71f0146a86f1"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ViewDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Control
        m_Control = asset.FindActionMap("Control", throwIfNotFound: true);
        m_Control_Move = m_Control.FindAction("Move", throwIfNotFound: true);
        m_Control_Jump = m_Control.FindAction("Jump", throwIfNotFound: true);
        m_Control_View = m_Control.FindAction("View", throwIfNotFound: true);
        m_Control_Zoom = m_Control.FindAction("Zoom", throwIfNotFound: true);
        m_Control_ViewDirection = m_Control.FindAction("ViewDirection", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Control
    private readonly InputActionMap m_Control;
    private List<IControlActions> m_ControlActionsCallbackInterfaces = new List<IControlActions>();
    private readonly InputAction m_Control_Move;
    private readonly InputAction m_Control_Jump;
    private readonly InputAction m_Control_View;
    private readonly InputAction m_Control_Zoom;
    private readonly InputAction m_Control_ViewDirection;
    public struct ControlActions
    {
        private @PlayerInputSystem m_Wrapper;
        public ControlActions(@PlayerInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Control_Move;
        public InputAction @Jump => m_Wrapper.m_Control_Jump;
        public InputAction @View => m_Wrapper.m_Control_View;
        public InputAction @Zoom => m_Wrapper.m_Control_Zoom;
        public InputAction @ViewDirection => m_Wrapper.m_Control_ViewDirection;
        public InputActionMap Get() { return m_Wrapper.m_Control; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlActions set) { return set.Get(); }
        public void AddCallbacks(IControlActions instance)
        {
            if (instance == null || m_Wrapper.m_ControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ControlActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @View.started += instance.OnView;
            @View.performed += instance.OnView;
            @View.canceled += instance.OnView;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
            @ViewDirection.started += instance.OnViewDirection;
            @ViewDirection.performed += instance.OnViewDirection;
            @ViewDirection.canceled += instance.OnViewDirection;
        }

        private void UnregisterCallbacks(IControlActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @View.started -= instance.OnView;
            @View.performed -= instance.OnView;
            @View.canceled -= instance.OnView;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
            @ViewDirection.started -= instance.OnViewDirection;
            @ViewDirection.performed -= instance.OnViewDirection;
            @ViewDirection.canceled -= instance.OnViewDirection;
        }

        public void RemoveCallbacks(IControlActions instance)
        {
            if (m_Wrapper.m_ControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IControlActions instance)
        {
            foreach (var item in m_Wrapper.m_ControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ControlActions @Control => new ControlActions(this);
    public interface IControlActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnView(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnViewDirection(InputAction.CallbackContext context);
    }
}
