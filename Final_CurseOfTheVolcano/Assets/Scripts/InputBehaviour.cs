using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.UI;

public class InputBehaviour : MonoBehaviour
{
    private Controls m_Controls;
    private CharacterControl m_CharacterControl;
    private Vector3 m_Rotation;

    private Gamepad m_CurrentController;
    static private List<PlayerInput.ActionEvent> m_SkipEvents = new List<PlayerInput.ActionEvent>();
    public void SetInputUser(InputUser inputUser, InputDevice controller)
    {
        m_CurrentController = (Gamepad)controller;

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
                    m_SkipEvents.Add(newEvent);

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

    static public void ResetSkipEvents()
    {
        m_SkipEvents.Clear();
    }
    private void Update()
    {
        //we want to be able to rotate the character in the selection screen but not when we are in the actual game
        if (m_CharacterControl.enabled == false)
        {
            m_CharacterControl.ApplyRotation(m_Rotation);
        }
    }

    static public List<PlayerInput.ActionEvent> SkipEvents
    { get { return m_SkipEvents; } }

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

    public void RumbleController(float strength, float length)
    {
        if (m_CurrentController != null)
        {
            m_CurrentController.SetMotorSpeeds(strength, strength);

            StartCoroutine(StopRumble(length));
        }
    }
    public void StopRumbleImmideately()
    {
        m_CurrentController.SetMotorSpeeds(0f, 0f);
    }
    IEnumerator StopRumble(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        StopRumbleImmideately();
    }
}
