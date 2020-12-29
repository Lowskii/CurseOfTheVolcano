using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrampolineBehaviour : MonoBehaviour
{
    [SerializeField] private float m_TrampolineForce;        
    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {                        
             other.gameObject.GetComponent<CharacterControl>().m_MoveDirection.y = m_TrampolineForce;
        }
        else
            return;
    }   
}

