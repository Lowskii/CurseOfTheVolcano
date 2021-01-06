using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private float m_SmoothSpeed;
    [SerializeField] private Vector3 m_Offset;

    void LateUpdate()
    {
        Vector3 desiredPos = Player.position + m_Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPos, m_SmoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
