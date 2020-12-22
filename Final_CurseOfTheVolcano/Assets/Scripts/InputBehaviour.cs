using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputBehaviour : MonoBehaviour
{
    Controls m_Controls;
    public void SetInputUser(InputUser inputUser)
    {
        m_Controls = (Controls)inputUser.actions;

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
                    newEvent.AddListener(Selection);
                    action.performed += newEvent.Invoke;
                    break;
                case "Select":
                    newEvent.AddListener(Select);
                    action.performed += newEvent.Invoke;
                    break;
                case "Back":
                    newEvent.AddListener(Back);
                    action.performed += newEvent.Invoke;
                    break;
                case "Movement":
                    newEvent.AddListener(Movement);
                    action.performed += newEvent.Invoke;
                    break;
                case "Jump":
                    newEvent.AddListener(Jump);
                    action.performed += newEvent.Invoke;
                    break;
                case "Push":
                    newEvent.AddListener(Push);
                    action.performed += newEvent.Invoke;
                    break;
                case "Interact":
                    newEvent.AddListener(Interact);
                    action.performed += newEvent.Invoke;
                    break;
                default:
                    Debug.Log("There is no functionality yet for action: " + action.name + " in action map: " + action.actionMap); 
                    break;
            }
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

    //Menu controls
     void Select(InputAction.CallbackContext value)
    {
         Debug.Log("select");
    }

     void Back(InputAction.CallbackContext value)
    {
        Debug.Log("Back");
    }

     void Selection(InputAction.CallbackContext value)
    {
       // Debug.Log("Selection: " + value.ReadValue<Vector2>());
    }


    //game controls

    void Jump(InputAction.CallbackContext value)
    {
        Debug.Log("Jump");
    }

    void Interact(InputAction.CallbackContext value)
    {
        Debug.Log("Interact");
    }

    void Push(InputAction.CallbackContext value)
    {
        Debug.Log("Push");
    }

    void Movement(InputAction.CallbackContext value)
    {
        //Debug.Log("Movement: " + value.ReadValue<Vector2>());
    }
}
