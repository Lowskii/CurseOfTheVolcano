using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineBehaviour : MonoBehaviour
{
    [SerializeField] private float m_TrampolineForce;
    private float m_JumpHeight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            //Start Player Jump
            //m_JumpHeight = other.gameObject.GetComponent<CharacterControl>().JumpHeight;
            //other.gameObject.GetComponent<CharacterControl>().JumpHeight = m_TrampolineForce;
            //other.gameObject.GetComponent<CharacterControl>().StartJump();
        }
        else
            return;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
           // other.gameObject.GetComponent<CharacterControl>().JumpHeight = m_JumpHeight;
        }
    }
}
