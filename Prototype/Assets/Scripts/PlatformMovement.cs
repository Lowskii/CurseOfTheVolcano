using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject[] Points;
    public float Speed;
    public float DelayTime;
    public bool Continuous;

    private Vector3 _currentTarget;
    private int pointsIndex;
    private float _tolerance;
    private float _delayStart;

    void Start()
    {
        pointsIndex = 0;
        if (Points.Length > 0) _currentTarget = Points[pointsIndex].transform.position;

        _tolerance = Speed * Time.deltaTime;
    }

    void Update()
    {
        if (this.transform.position != _currentTarget) MovePlatform();
        else if (Continuous) UpdateTarget();
    }

    private void MovePlatform()
    {
        Vector3 heading = _currentTarget - this.transform.position;

        this.transform.position += (heading / heading.magnitude) * Speed * Time.deltaTime;

        if (heading.magnitude < _tolerance)
        {
            this.transform.position = _currentTarget;
            _delayStart = Time.time;
        }
    }

    private void UpdateTarget()
    {
        if (Time.time - _delayStart > DelayTime)
        {
            NextPlatform();
        }
    }
    private void NextPlatform()
    {
        pointsIndex++;
        if (pointsIndex >= Points.Length) pointsIndex = 0;

        _currentTarget = Points[pointsIndex].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = this.transform;
        if (!Continuous) NextPlatform();
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
