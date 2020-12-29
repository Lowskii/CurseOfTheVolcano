using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControlBehaviour : MonoBehaviour
{
    [SerializeField] private SpinningPlatformBehaviour[] m_platforms;
    [SerializeField] private bool m_IsPlatformActivated;
    [SerializeField] private float m_ResetDelay;

    private float m_DelayTimer;    
    private CharacterControl m_Player;   

    void Update()
    {
        if (m_IsPlatformActivated) ActivatePlatform();
        else ResetPlatform();
    }

    private void ActivatePlatform()
    {        
        foreach (var platform in m_platforms)
        {
            platform.IsSpinning = true;
        }
        StartDelayTimer();
    }

    private void ResetPlatform()
    {
        foreach (var platform in m_platforms)
        {
            platform.IsSpinning = false;
        }
    }

    private void StartDelayTimer()
    {
        if (m_IsPlatformActivated)
        {
            m_DelayTimer += Time.deltaTime;
            if (m_DelayTimer > m_ResetDelay)
            {
                m_IsPlatformActivated = false;
                m_DelayTimer -= m_ResetDelay;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null && m_Player == null)
        {
            m_Player = other.gameObject.GetComponent<CharacterControl>();            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (m_Player != null)
        {            
            if (m_Player.IsInteractPressed)
            {                
                m_IsPlatformActivated = true;
            }
        }
        else
            m_Player = other.gameObject.GetComponent<CharacterControl>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (m_Player != null)
        {            
            m_Player = null;
        }
    }
}

