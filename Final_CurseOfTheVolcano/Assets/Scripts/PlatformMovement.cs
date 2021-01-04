using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject[] Points;
    public float Speed;
    public float DelayTime;
    public bool Continuous;

    private Vector3 m_CurrentTarget;
    private int m_PointsIndex;
    private float m_Tolerance;
    private float m_DelayStart;
    private bool m_Moving;
    public bool IsMoving => m_Moving;

    private Vector3 m_Movement;
    void Start()
    {
        m_PointsIndex = 0;
        if (Points.Length > 0) m_CurrentTarget = Points[m_PointsIndex].transform.position;

        m_Tolerance = Speed * Time.deltaTime;

        m_Moving = false;
    }

    void Update()
    {
        if ((this.transform.position - m_CurrentTarget).magnitude > Speed * Time.deltaTime) MovePlatform();
        else if (Continuous) UpdateTarget();
    }

    private void MovePlatform()
    {
        Vector3 heading = m_CurrentTarget - this.transform.position;

        m_Movement = heading.normalized * Speed * Time.deltaTime;

        this.transform.position += m_Movement;

        if (heading.magnitude < m_Tolerance)
        {
            this.transform.position = m_CurrentTarget;
            m_DelayStart = Time.time;
            m_Moving = false;
        }
        else m_Moving = true;
    }

    private void UpdateTarget()
    {
        if (Time.time - m_DelayStart > DelayTime)
        {
            NextPlatform();
        }
    }
    private void NextPlatform()
    {
        m_PointsIndex++;
        if (m_PointsIndex >= Points.Length) m_PointsIndex = 0;

        m_CurrentTarget = Points[m_PointsIndex].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {        
        other.transform.parent = this.transform;               
        if (!Continuous && !m_Moving) NextPlatform();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<CharacterControl>().AddExternalMovement(m_Movement);
        }
    }

}
