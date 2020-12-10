using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SpawnBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] _SpawnPoints = new GameObject[4];
    InputDevice _CurrentController;

    int _PlayerID = -1; //we increment this each spawn but the arrays start at 0
    int _PlayersInGame = 0;

    UnityEvent _PlayerDied = new UnityEvent();

    public UnityEvent PlayerDiedEvent
    { get { return _PlayerDied; } }

    public int PlayerId
    {
        get { return _PlayerID; }
    }

    public Vector3 GetSpawnPosition()
    {
        return _SpawnPoints[_PlayerID].transform.position;
    }

    public InputDevice CurrentController
    { get { return _CurrentController; } }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        _CurrentController = playerInput.devices[0];
        ++_PlayerID;
        ++_PlayersInGame;
    }

    public void RemovePlayer()
    {
        --_PlayersInGame;
        PlayerDiedEvent.Invoke();
    }

    public int PlayersLeft
    { get { return _PlayersInGame; } }


}
