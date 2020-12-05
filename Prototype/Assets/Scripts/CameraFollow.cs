using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Lava;
    public Transform Player;
    private Vector3 _velocity;

    [SerializeField]
    private float _smoothTime;

    [SerializeField]
    private Vector3 _offset;

    private void Update()
    {
        CalculateDistance();
        MoveCamera();        
    }
    private void CalculateDistance()
    {
        Vector2 lavaViewPortPosition = Camera.main.WorldToViewportPoint(Lava.position);
        Vector2 playerViewPortPosition = Camera.main.WorldToViewportPoint(Player.position);
        float distance = playerViewPortPosition.y - lavaViewPortPosition.y;
        Debug.Log(distance);
        if(distance > .6f)
        {
            //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + new Vector3(0, 0, -10), Time.deltaTime);
            Vector3 newPosition = Camera.main.transform.position + new Vector3(0, 0, -50);
            Camera.main.transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, 1f);
        }
       
    }
    private void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime);
    }
    private Vector3 GetCenterPoint()
    {        
        var bounds = new Bounds(Lava.position, Vector3.zero);
        bounds.Encapsulate(Player.position);

        return bounds.center;
    }

}
