using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControlBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject m_Bridge;
    [SerializeField] private bool m_IsBridgeActivated;
    [SerializeField] private float m_ResetDelay;

    private float m_DelayTimer;
    private Vector3 m_StartPosition;
    private Quaternion m_StartRotation;


    void Start()
    {
        m_StartPosition = m_Bridge.transform.position;
        m_StartRotation = m_Bridge.transform.rotation;
    }
    
    void Update()
    {
        if (m_IsBridgeActivated) DropBridge();
        else ResetBridge();
    }

    private void DropBridge()
    {
        var rB = m_Bridge.GetComponent<Rigidbody>();
        if (!m_IsBridgeActivated)
        {
            rB.isKinematic = true;
            return;
        }

        rB.isKinematic = false;
        StartDelayTimer();
    }

    private void ResetBridge()
    {
        var rB = m_Bridge.GetComponent<Rigidbody>();
        rB.velocity = Vector3.zero;
        m_Bridge.transform.position = m_StartPosition;
        m_Bridge.transform.rotation = m_StartRotation;
    }

    private void StartDelayTimer()
    {
        if (m_IsBridgeActivated)
        {
            m_DelayTimer += Time.deltaTime;
            if (m_DelayTimer > m_ResetDelay)
            {
                m_IsBridgeActivated = false;
                m_DelayTimer -= m_ResetDelay;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.GetComponent<CharacterControl>() != null)
        //{
        //    _player = other.gameObject.GetComponent<CharacterControl>();
        //    if (_player.Interact)
        //    {
        //        m_IsBridgeActivated = true;
        //    }
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        //if (_player != null)
        //{
        //    _player.Interact = false;
        //    _player = null;
        //}
    }
}
