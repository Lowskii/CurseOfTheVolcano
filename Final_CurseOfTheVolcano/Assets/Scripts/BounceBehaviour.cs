using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_BounceForce;
    private Transform m_Player;
    private bool m_IsPushing;

    private void FixedUpdate()
    {
        PushPlayers();       
    }

    private void PushPlayers()
    {
        if (m_Player != null && m_IsPushing)
        {           
            StartCoroutine(PushBack());
            m_Player.transform.position += m_BounceForce * Time.fixedDeltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<MovingObstacleBehaviour>().IsMoving)
        {
            m_Player = other.gameObject.transform;
            m_IsPushing = true;
        }
              
    }
    IEnumerator PushBack()
    {
        while (m_IsPushing)
        {
            yield return new WaitForSeconds(.1f);
            m_Player = null;
            m_IsPushing = false;
        }
    }
}
