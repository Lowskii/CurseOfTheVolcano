// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""GameControls"",
            ""id"": ""7126f1dd-5fce-4d88-b671-edae3aeb0ff6"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""2723e1db-71fd-4a51-b0b6-8eaa93afe85e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""eba551a1-7814-4299-ba97-a1f363239a37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""ebced09c-2178-4e4f-8335-511f25ebd9b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Push"",
                    ""type"": ""Button"",
                    ""id"": ""be455ec2-927d-4211-899b-22343f8dd312"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""10520fc6-d0e8-4b51-b34d-edeb644c2446"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83b3051c-d109-45f9-ba1c-9e07cd49419b"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2d7930e-bf74-4a44-93f1-c4c4636bff27"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c6cbba1-6cd9-42a7-8bf3-11682f8eb5f6"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39853de7-bf62-4c09-9685-46e5f5f82791"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Push"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MenuControls"",
            ""id"": ""2acd94c9-b16b-45ab-96aa-61eef6063e9b"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1fa63b5d-5ae5-4f51-ba36-ad2cc403e125"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Selection"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c0fb1228-74b2-47fb-a045-cc081766d66f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""77391dc7-9cf5-4cf7-901e-d9030183c258"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""f6b2542a-c294-4826-8aaa-6064ded44a77"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e2059797-e0f0-4167-8d5e-817a4774cd18"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6881743-aa5a-4209-8c92-70aff56d4767"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d580fb44-0cfa-4db0-ae8b-5f744f412ec4"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73b81e0f-e409-4ce4-8f78-23ee1db5dd54"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameControls
        m_GameControls = asset.FindActionMap("GameControls", throwIfNotFound: true);
        m_GameControls_Movement = m_GameControls.FindAction("Movement", throwIfNotFound: true);
        m_GameControls_Jump = m_GameControls.FindAction("Jump", throwIfNotFound: true);
        m_GameControls_Interact = m_GameControls.FindAction("Interact", throwIfNotFound: true);
        m_GameControls_Push = m_GameControls.FindAction("Push", throwIfNotFound: true);
        // MenuControls
        m_MenuControls = asset.FindActionMap("MenuControls", throwIfNotFound: true);
        m_MenuControls_Select = m_MenuControls.FindAction("Select", throwIfNotFound: true);
        m_MenuControls_Selection = m_MenuControls.FindAction("Selection", throwIfNotFound: true);
        m_MenuControls_Start = m_MenuControls.FindAction("Start", throwIfNotFound: true);
        m_MenuControls_Rotate = m_MenuControls.FindAction("Rotate", throwIfNotFound: true);
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

    // GameControls
    private readonly InputActionMap m_GameControls;
    private IGameControlsActions m_GameControlsActionsCallbackInterface;
    private readonly InputAction m_GameControls_Movement;
    private readonly InputAction m_GameControls_Jump;
    private readonly InputAction m_GameControls_Interact;
    private readonly InputAction m_GameControls_Push;
    public struct GameControlsActions
    {
        private @Controls m_Wrapper;
        public GameControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_GameControls_Movement;
        public InputAction @Jump => m_Wrapper.m_GameControls_Jump;
        public InputAction @Interact => m_Wrapper.m_GameControls_Interact;
        public InputAction @Push => m_Wrapper.m_GameControls_Push;
        public InputActionMap Get() { return m_Wrapper.m_GameControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameControlsActions set) { return set.Get(); }
        public void SetCallbacks(IGameControlsActions instance)
        {
            if (m_Wrapper.m_GameControlsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnJump;
                @Interact.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnInteract;
                @Push.started -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnPush;
                @Push.performed -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnPush;
                @Push.canceled -= m_Wrapper.m_GameControlsActionsCallbackInterface.OnPush;
            }
            m_Wrapper.m_GameControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Push.started += instance.OnPush;
                @Push.performed += instance.OnPush;
                @Push.canceled += instance.OnPush;
            }
        }
    }
    public GameControlsActions @GameControls => new GameControlsActions(this);

    // MenuControls
    private readonly InputActionMap m_MenuControls;
    private IMenuControlsActions m_MenuControlsActionsCallbackInterface;
    private readonly InputAction m_MenuControls_Select;
    private readonly InputAction m_MenuControls_Selection;
    private readonly InputAction m_MenuControls_Start;
    private readonly InputAction m_MenuControls_Rotate;
    public struct MenuControlsActions
    {
        private @Controls m_Wrapper;
        public MenuControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_MenuControls_Select;
        public InputAction @Selection => m_Wrapper.m_MenuControls_Selection;
        public InputAction @Start => m_Wrapper.m_MenuControls_Start;
        public InputAction @Rotate => m_Wrapper.m_MenuControls_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_MenuControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuControlsActions set) { return set.Get(); }
        public void SetCallbacks(IMenuControlsActions instance)
        {
            if (m_Wrapper.m_MenuControlsActionsCallbackInterface != null)
            {
                @Select.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSelect;
                @Selection.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSelection;
                @Selection.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSelection;
                @Selection.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnSelection;
                @Start.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnStart;
                @Rotate.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_MenuControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Selection.started += instance.OnSelection;
                @Selection.performed += instance.OnSelection;
                @Selection.canceled += instance.OnSelection;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public MenuControlsActions @MenuControls => new MenuControlsActions(this);
    public interface IGameControlsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnPush(InputAction.CallbackContext context);
    }
    public interface IMenuControlsActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnSelection(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
}
