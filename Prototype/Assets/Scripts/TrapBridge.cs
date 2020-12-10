using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrapBridge : MonoBehaviour
{
    private CharacterControl _player;
    [SerializeField]
    private GameObject _bridge;
    [SerializeField]
    private float _resetDelay;
    private float _delayTimer;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    [SerializeField]
    private bool _buttonPressed;

    
    private void Start()
    {
        _startPosition = _bridge.transform.position;
        _startRotation = _bridge.transform.rotation;
    }   
    private void Update()
    {
        if (_buttonPressed)
            DropBridge();
        else
            ResetBridge();

        //Debug.Log(_buttonPressed);
    }

    private void DropBridge()
    {
       var rB = _bridge.GetComponent<Rigidbody>();
        if (!_buttonPressed)
        {            
            rB.isKinematic = true;           
            return;
        }  
        
        rB.isKinematic = false;
        StartDelayTimer();
    }

    private void ResetBridge()
    {        
        var rB = _bridge.GetComponent<Rigidbody>();
        rB.velocity = Vector3.zero;
        _bridge.transform.position = _startPosition;
        _bridge.transform.rotation = _startRotation;        
    }

    private void OnTriggerStay(Collider other)
    {        
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            _player = other.gameObject.GetComponent<CharacterControl>();
            if (_player.Interact)
            {
                _buttonPressed = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {        
        if (_player != null)
        {
            _player.Interact = false;
            _player = null;
        }
    }

    private void StartDelayTimer()
    {
        if (_buttonPressed)
        {
            //_player.Interact = false;
            _delayTimer += Time.deltaTime;
            if (_delayTimer > _resetDelay)
            {
                _buttonPressed = false;
                _delayTimer -= _resetDelay;                
            }
        }
    }    
}
