using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera m_DesignatedCamera = null;
    void LateUpdate()
    {
        if(m_DesignatedCamera == null)
            transform.LookAt(transform.position + Camera.main.transform.forward);
        else
            transform.LookAt(transform.position + m_DesignatedCamera.transform.forward);
    }
}
