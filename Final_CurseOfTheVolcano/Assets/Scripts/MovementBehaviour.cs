using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBehaviour : MonoBehaviour
{
    public void Jump(InputAction.CallbackContext value)
    {
        Debug.Log("Jump");
    }
    public void Movement(InputAction.CallbackContext value)
    {
        //Debug.Log("Movement: " + value.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        //handle the fysics and movement
    }
}
