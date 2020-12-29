using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform m_Player;

    [SerializeField] float m_SmoothSpeed;
    [SerializeField] Vector3 m_Offset;

    void LateUpdate()
    {
        Vector3 desiredPos = m_Player.position + m_Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPos, m_SmoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
