using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class InputBehaviour : MonoBehaviour
{
    static int _PlayersAlive = 0;

    Vector2 _RotationVector;

    UnityEvent _StartJumpEvent = new UnityEvent();
    UnityEvent _CancelJumpEvent = new UnityEvent();
    UnityEvent _StartInteractEvent = new UnityEvent();
   static UnityEvent _PlayerDiedEvent = new UnityEvent();

    private Controls _Controls;
    private PlayerConfiguration _PlayerConfig;
    public UnityEvent StartJumpEvent
    { get { return _StartJumpEvent; } }

    public UnityEvent CancelJumpEvent
    { get { return _CancelJumpEvent; } }

    public UnityEvent StartInteractEvent
    { get { return _StartInteractEvent; } }

    static public UnityEvent PlayerDiedEvent
    { get { return _PlayerDiedEvent; } }
    public Vector2 RotationVector
    {
        get { return _RotationVector; }
    }

    private void Awake()
    {
        _Controls = new Controls();
    }

    public void InitialiazePlayer(PlayerConfiguration pc)
    {
        _PlayerConfig = pc;

        //set correct color
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int t = 0; t < meshRenderers.Length; t++)
        {
            meshRenderers[t].material = _PlayerConfig.PlayerMaterial;
        }

        //setup input
        _PlayerConfig.Input.onActionTriggered += Input_OnActionTriggered;

        ++_PlayersAlive;
    }

    private void Input_OnActionTriggered(InputAction.CallbackContext obj)
    {
        if(obj.action.name == _Controls.PlayerControls.Move.name)
        {
            OnMove(obj);
        }

        if (obj.action.name == _Controls.PlayerControls.Interact.name)
        {
            OnInteract();
        }

        if (obj.action.name == _Controls.PlayerControls.Jump.name)
        {
            OnJump();
        }
    }
    //EVENTS
    public void OnMove(InputAction.CallbackContext value)
    {
        _RotationVector = value.ReadValue<Vector2>();
    }
    public void OnJump()
    {
        _StartJumpEvent.Invoke();
    }

    public void OnInteract()
    {
        _StartInteractEvent.Invoke();
    }

    static public void PlayerDied()
    {
        --_PlayersAlive;
        _PlayerDiedEvent.Invoke();
    }

    static public int PlayersAlive
    { get { return _PlayersAlive; } }

}


