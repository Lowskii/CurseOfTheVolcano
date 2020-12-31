using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.UI;

public class InputBehaviour : MonoBehaviour
{
    Controls m_Controls;
    CharacterControl m_CharacterControl;
    Vector3 m_Rotation;
    public void SetInputUser(InputUser inputUser)
    {
        m_Controls = (Controls)inputUser.actions;

        InputSystemUIInputModule inputModule = transform.parent.parent.GetComponentInChildren<InputSystemUIInputModule>();

        m_CharacterControl = GetComponentInChildren<CharacterControl>();

        foreach (InputAction action in inputUser.actions)
        {
            PlayerInput.ActionEvent newEvent = new PlayerInput.ActionEvent(action);


            /* ADD one of these to the case to invoke the action
            action.performed += newEvent.Invoke;
            action.canceled += newEvent.Invoke;
            action.started += newEvent.Invoke;
             */
            switch (action.name)
            {
                case "Selection":

                    if (inputModule != null)
                    {
                        InputActionReference selectionRef = ScriptableObject.CreateInstance<InputActionReference>();
                        selectionRef.Set(action);

                        inputModule.move = selectionRef;
                    }
                    break;
                case "Select":
                    action.performed += newEvent.Invoke;
                    if (inputModule != null)
                    {
                        InputActionReference selectRef = ScriptableObject.CreateInstance<InputActionReference>();
                        selectRef.Set(action);

                        inputModule.submit = selectRef;
                    }
                    break;
                case "Start":
                    break;
                case "Rotate":
                    newEvent.AddListener(Rotate);
                    action.started += newEvent.Invoke;
                    action.performed += newEvent.Invoke;
                    action.canceled += newEvent.Invoke;
                    break;
                case "Movement":
                    newEvent.AddListener(m_CharacterControl.Movement);
                    action.started += newEvent.Invoke;
                    action.performed += newEvent.Invoke;
                    action.canceled += newEvent.Invoke;
                    break;
                case "Jump":
                    newEvent.AddListener(m_CharacterControl.Jump);
                    action.performed += newEvent.Invoke;
                    break;
                case "Push":
                    newEvent.AddListener(m_CharacterControl.Push);
                    action.performed += newEvent.Invoke;
                    break;
                case "Interact":
                    newEvent.AddListener(m_CharacterControl.Interact);
                    action.performed += newEvent.Invoke;
                    break;
                default:
                    Debug.Log("There is no functionality yet for action: " + action.name + " in action map: " + action.actionMap);
                    break;
            }
        }
    }

    private void Update()
    {
        //we want to be able to rotate the character in the selection screen but not when we are in the actual game
        if (m_CharacterControl.enabled == false)
        {
            m_CharacterControl.ApplyRotation(m_Rotation);
        }
    }
    public void SwitchToGameActionMapping()
    {
        m_Controls.GameControls.Enable();
        m_Controls.MenuControls.Disable();
    }

    public void SwitchToMenuActionMapping()
    {
        m_Controls.GameControls.Disable();
        m_Controls.MenuControls.Enable();
    }

    private void Rotate(InputAction.CallbackContext value)
    {
        Vector2 rotation = value.ReadValue<Vector2>();

        m_Rotation = new Vector3(rotation.x, 0, rotation.y);

        m_Rotation.Normalize();
    }

}
