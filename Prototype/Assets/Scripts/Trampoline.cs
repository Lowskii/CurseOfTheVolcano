using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    private float _trampolineForce;    
    private float _jumpHeight;    
    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            _jumpHeight = other.gameObject.GetComponent<CharacterControl>().JumpHeight;
            other.gameObject.GetComponent<CharacterControl>().JumpHeight = _trampolineForce;
            other.gameObject.GetComponent<CharacterControl>().StartJump();            
        }
        else
            return;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            other.gameObject.GetComponent<CharacterControl>().JumpHeight = _jumpHeight;
        }
    }
}
