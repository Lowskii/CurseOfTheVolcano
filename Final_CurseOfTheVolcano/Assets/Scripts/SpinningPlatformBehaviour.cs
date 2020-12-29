using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlatformBehaviour : MonoBehaviour
{
    [SerializeField] private float m_TurnSpeed;
    public bool IsSpinning;
  
    void FixedUpdate()
    {
        if (IsSpinning) this.transform.Rotate(Vector3.up * m_TurnSpeed * Time.deltaTime);
    }

    public void ActivateSpinning()
    {
        IsSpinning = true;
    }

    public void DeactivateSpinning()
    {
        IsSpinning = false;
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
