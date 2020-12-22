using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlatformBehaviour : MonoBehaviour
{
    [SerializeField] private float m_TurnSpeed;
    [SerializeField] private bool m_IsSpinning;
  
    void Update()
    {
        if (m_IsSpinning) this.transform.Rotate(Vector3.up * m_TurnSpeed * Time.deltaTime);
    }

    public void ActivateSpinning()
    {
        m_IsSpinning = true;
    }

    public void DeactivateSpinning()
    {
        m_IsSpinning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = this.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
