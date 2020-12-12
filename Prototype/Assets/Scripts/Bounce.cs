using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField]
    private float _bounceForce;
    private Transform _player;
    private bool _pushing;

    private void Update()
    {
        PushPlayers();
    }
    private void PushPlayers()
    {    if (_player != null && _pushing)
        {
            StartCoroutine(PushBack());
            _player.transform.position -= Vector3.forward * Time.deltaTime * _bounceForce;
        }
    }    
    private void OnTriggerEnter(Collider other)
    {        
        if(GetComponent<PlatformMovement>().Moving)
        {
            _player = other.gameObject.transform;
            _pushing = true;
        }
    }      
    IEnumerator PushBack()
    {
        while (_pushing)
        {
            yield return new WaitForSeconds(.1f);
            _player = null;
            _pushing = false;
        }
    }
}
