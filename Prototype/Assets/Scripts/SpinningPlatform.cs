using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPlatform : MonoBehaviour
{
    [SerializeField]
    private float _turnSpeed;    
  
    void Update()
    {        
        this.transform.Rotate(Vector3.up * _turnSpeed * Time.deltaTime);      
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
