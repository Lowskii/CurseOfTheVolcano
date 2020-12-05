using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float Speed;
    public float MaxHeight;
    void Update()
    {
        if(transform.position.y < MaxHeight)
        {
            this.transform.position += Vector3.up * Speed * Time.deltaTime;
        }        
    }
}
