using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField]
    private Vector3 _bounceForce;
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
            _player.transform.position += _bounceForce * Time.deltaTime;
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
