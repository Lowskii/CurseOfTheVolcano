using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class InputBehaviour : MonoBehaviour
{
    Vector2 _RotationVector;

    UnityEvent _StartJumpEvent = new UnityEvent();
    UnityEvent _CancelJumpEvent = new UnityEvent();
    UnityEvent _StartInteractEvent = new UnityEvent();
    UnityEvent _StartPushEvent = new UnityEvent();
    UnityEvent _EndPushEvent = new UnityEvent();


    public UnityEvent StartJumpEvent
    { get { return _StartJumpEvent; } }

    public UnityEvent CancelJumpEvent
    { get { return _CancelJumpEvent; } }

    public UnityEvent StartPushEvent
    { get { return _StartPushEvent; } }

    public UnityEvent EndPushEvent
    { get { return _EndPushEvent; } }

    public UnityEvent StartInteractEvent
    { get { return _StartInteractEvent; } }

        public Vector2 RotationVector
    {
        get { return _RotationVector; }
    }

    //EVENTS
    public void OnMove(InputValue value)
    {
        _RotationVector = value.Get<Vector2>();
    }
    public void OnJump()
    {
        _StartJumpEvent.Invoke();
    }
    public void OnPush()
    {
        _StartPushEvent.Invoke();
    }

    public void OnInteract()
    {
        _StartInteractEvent.Invoke();
    }
}


