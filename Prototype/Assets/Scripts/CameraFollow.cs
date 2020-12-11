using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{    
    public Transform Player;
    private Vector3 _velocity;

    [SerializeField]
    private float _smoothTime;

    [SerializeField]
    private Vector3 _offset;   

    private void Update()
    {
        //if (GameObject.Find("TempPlayer(Clone)"))
        //{
        //    Player = FindObjectOfType<CharacterControl>().gameObject.transform;
        //}
        if (Player != null)
        {
            MoveCamera();
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
        var bounds = new Bounds(Player.position, Vector3.zero);
        return bounds.center;
    }

}
