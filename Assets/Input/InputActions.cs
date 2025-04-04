//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Input/InputActions.inputactions
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

public partial class @InputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerMovements"",
            ""id"": ""8a31ecb0-b043-4129-815d-ce7b88eb48b9"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""275c3def-191c-472f-9519-1ee874e039f1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f6e8e78b-7c14-4256-82a4-abc1393b6a22"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""e80f6f63-949a-4863-91bd-0bca7212cec1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""eb9e34f0-7743-427f-80b9-52d8e6b9ab34"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""89aecf4d-ed3e-47db-a6e8-c6449f14b40f"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c67b851-68b0-4619-8371-0cd2a9f26ade"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0b0041b-69db-4908-80b0-7ed2b8089426"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""275d1545-0b17-4b6d-a9f7-3232a91de7ff"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""ControlScheme"",
            ""bindingGroup"": ""ControlScheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerMovements
        m_PlayerMovements = asset.FindActionMap("PlayerMovements", throwIfNotFound: true);
        m_PlayerMovements_Movement = m_PlayerMovements.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMovements_Rotation = m_PlayerMovements.FindAction("Rotation", throwIfNotFound: true);
        m_PlayerMovements_Shoot = m_PlayerMovements.FindAction("Shoot", throwIfNotFound: true);
        m_PlayerMovements_Throw = m_PlayerMovements.FindAction("Throw", throwIfNotFound: true);
    }

    ~@InputActions()
    {
        UnityEngine.Debug.Assert(!m_PlayerMovements.enabled, "This will cause a leak and performance issues, InputActions.PlayerMovements.Disable() has not been called.");
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

    // PlayerMovements
    private readonly InputActionMap m_PlayerMovements;
    private List<IPlayerMovementsActions> m_PlayerMovementsActionsCallbackInterfaces = new List<IPlayerMovementsActions>();
    private readonly InputAction m_PlayerMovements_Movement;
    private readonly InputAction m_PlayerMovements_Rotation;
    private readonly InputAction m_PlayerMovements_Shoot;
    private readonly InputAction m_PlayerMovements_Throw;
    public struct PlayerMovementsActions
    {
        private @InputActions m_Wrapper;
        public PlayerMovementsActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovements_Movement;
        public InputAction @Rotation => m_Wrapper.m_PlayerMovements_Rotation;
        public InputAction @Shoot => m_Wrapper.m_PlayerMovements_Shoot;
        public InputAction @Throw => m_Wrapper.m_PlayerMovements_Throw;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovements; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementsActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerMovementsActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerMovementsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerMovementsActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Rotation.started += instance.OnRotation;
            @Rotation.performed += instance.OnRotation;
            @Rotation.canceled += instance.OnRotation;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @Throw.started += instance.OnThrow;
            @Throw.performed += instance.OnThrow;
            @Throw.canceled += instance.OnThrow;
        }

        private void UnregisterCallbacks(IPlayerMovementsActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Rotation.started -= instance.OnRotation;
            @Rotation.performed -= instance.OnRotation;
            @Rotation.canceled -= instance.OnRotation;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @Throw.started -= instance.OnThrow;
            @Throw.performed -= instance.OnThrow;
            @Throw.canceled -= instance.OnThrow;
        }

        public void RemoveCallbacks(IPlayerMovementsActions instance)
        {
            if (m_Wrapper.m_PlayerMovementsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerMovementsActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerMovementsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerMovementsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerMovementsActions @PlayerMovements => new PlayerMovementsActions(this);
    private int m_ControlSchemeSchemeIndex = -1;
    public InputControlScheme ControlSchemeScheme
    {
        get
        {
            if (m_ControlSchemeSchemeIndex == -1) m_ControlSchemeSchemeIndex = asset.FindControlSchemeIndex("ControlScheme");
            return asset.controlSchemes[m_ControlSchemeSchemeIndex];
        }
    }
    public interface IPlayerMovementsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
    }
}
