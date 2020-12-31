using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrampolineBehaviour : MonoBehaviour
{
    [SerializeField] private float m_TrampolineForce;        
    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Player")
        {                        
             other.gameObject.GetComponent<CharacterControl>().m_MoveDirection.y = m_TrampolineForce;
             other.gameObject.GetComponent<InputBehaviour>().RumbleController(0.45f,0.25f);
        }
        else
            return;
    }   
}

