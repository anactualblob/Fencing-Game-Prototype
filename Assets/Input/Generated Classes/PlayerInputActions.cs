// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Fencing"",
            ""id"": ""12bb58e1-3e89-4ab0-bbad-00a0f10d6f35"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Value"",
                    ""id"": ""9429a457-7d51-4768-bd6e-40198476936b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""494d4a9f-d749-4461-b401-59dba8b8cbdf"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StopFencing"",
                    ""type"": ""Button"",
                    ""id"": ""dc840a52-ca77-42c3-bf39-81c7883922fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""62cc83d0-dfd7-4988-a6e0-245bbb23f26a"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15276a2e-33f7-4f79-89f4-98066043cc58"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f81fb9ca-1771-4723-a6ba-baaa07f0a3d2"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5865df6-bbe7-42b0-bb9e-58faa0f689a1"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""StopFencing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MovingAround"",
            ""id"": ""28bf7875-dead-4287-9c37-df03d4f995d1"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""29511eee-60c1-4c90-8833-2b4417a67914"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""8219f8c3-bac8-4867-9055-cedc50d05420"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartFencing"",
                    ""type"": ""Button"",
                    ""id"": ""420e8d5f-b2be-45a2-a4ee-a1a9fa2d7222"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2a979328-f3b1-453c-815f-4ade0ddddaf3"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d1b14b8-d0c3-40db-a5fb-89663b4be842"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b396745-b6e6-4c19-a1d8-6ed815af8db1"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""StartFencing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
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
        // Fencing
        m_Fencing = asset.FindActionMap("Fencing", throwIfNotFound: true);
        m_Fencing_Attack = m_Fencing.FindAction("Attack", throwIfNotFound: true);
        m_Fencing_Aim = m_Fencing.FindAction("Aim", throwIfNotFound: true);
        m_Fencing_StopFencing = m_Fencing.FindAction("StopFencing", throwIfNotFound: true);
        // MovingAround
        m_MovingAround = asset.FindActionMap("MovingAround", throwIfNotFound: true);
        m_MovingAround_Move = m_MovingAround.FindAction("Move", throwIfNotFound: true);
        m_MovingAround_Camera = m_MovingAround.FindAction("Camera", throwIfNotFound: true);
        m_MovingAround_StartFencing = m_MovingAround.FindAction("StartFencing", throwIfNotFound: true);
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

    // Fencing
    private readonly InputActionMap m_Fencing;
    private IFencingActions m_FencingActionsCallbackInterface;
    private readonly InputAction m_Fencing_Attack;
    private readonly InputAction m_Fencing_Aim;
    private readonly InputAction m_Fencing_StopFencing;
    public struct FencingActions
    {
        private @PlayerInputActions m_Wrapper;
        public FencingActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Fencing_Attack;
        public InputAction @Aim => m_Wrapper.m_Fencing_Aim;
        public InputAction @StopFencing => m_Wrapper.m_Fencing_StopFencing;
        public InputActionMap Get() { return m_Wrapper.m_Fencing; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FencingActions set) { return set.Get(); }
        public void SetCallbacks(IFencingActions instance)
        {
            if (m_Wrapper.m_FencingActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_FencingActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_FencingActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_FencingActionsCallbackInterface.OnAttack;
                @Aim.started -= m_Wrapper.m_FencingActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_FencingActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_FencingActionsCallbackInterface.OnAim;
                @StopFencing.started -= m_Wrapper.m_FencingActionsCallbackInterface.OnStopFencing;
                @StopFencing.performed -= m_Wrapper.m_FencingActionsCallbackInterface.OnStopFencing;
                @StopFencing.canceled -= m_Wrapper.m_FencingActionsCallbackInterface.OnStopFencing;
            }
            m_Wrapper.m_FencingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @StopFencing.started += instance.OnStopFencing;
                @StopFencing.performed += instance.OnStopFencing;
                @StopFencing.canceled += instance.OnStopFencing;
            }
        }
    }
    public FencingActions @Fencing => new FencingActions(this);

    // MovingAround
    private readonly InputActionMap m_MovingAround;
    private IMovingAroundActions m_MovingAroundActionsCallbackInterface;
    private readonly InputAction m_MovingAround_Move;
    private readonly InputAction m_MovingAround_Camera;
    private readonly InputAction m_MovingAround_StartFencing;
    public struct MovingAroundActions
    {
        private @PlayerInputActions m_Wrapper;
        public MovingAroundActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_MovingAround_Move;
        public InputAction @Camera => m_Wrapper.m_MovingAround_Camera;
        public InputAction @StartFencing => m_Wrapper.m_MovingAround_StartFencing;
        public InputActionMap Get() { return m_Wrapper.m_MovingAround; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovingAroundActions set) { return set.Get(); }
        public void SetCallbacks(IMovingAroundActions instance)
        {
            if (m_Wrapper.m_MovingAroundActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnMove;
                @Camera.started -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnCamera;
                @StartFencing.started -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnStartFencing;
                @StartFencing.performed -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnStartFencing;
                @StartFencing.canceled -= m_Wrapper.m_MovingAroundActionsCallbackInterface.OnStartFencing;
            }
            m_Wrapper.m_MovingAroundActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @StartFencing.started += instance.OnStartFencing;
                @StartFencing.performed += instance.OnStartFencing;
                @StartFencing.canceled += instance.OnStartFencing;
            }
        }
    }
    public MovingAroundActions @MovingAround => new MovingAroundActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IFencingActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnStopFencing(InputAction.CallbackContext context);
    }
    public interface IMovingAroundActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnStartFencing(InputAction.CallbackContext context);
    }
}
