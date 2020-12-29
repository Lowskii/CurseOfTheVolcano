using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrampolineBehaviour : MonoBehaviour
{
    [SerializeField] private float m_TrampolineForce;
    private float m_JumpHeight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {            
            m_JumpHeight = other.gameObject.GetComponent<CharacterControl>().JumpHeight;
            other.gameObject.GetComponent<CharacterControl>().JumpHeight = m_TrampolineForce;
            //other.gameObject.GetComponent<CharacterControl>().Jump(InputAction.CallbackContext value);
        }
        else
            return;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
           other.gameObject.GetComponent<CharacterControl>().JumpHeight = m_JumpHeight;
        }
    }
}
