﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] _SpawnPoints = new Transform[4];
    [SerializeField] private GameObject _PlayerPrefab;    

    private void Start()
    {
        //spawn players
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(_PlayerPrefab, _SpawnPoints[i].position, _SpawnPoints[i].rotation);          
            player.GetComponent<CharacterControl>().InitialiazePlayer(playerConfigs[i]);
            playerConfigs[i].Input.gameObject.GetComponentInChildren<CameraFollow>().Player = player.transform;            
        }

        //disable joining
        PlayerInputManager InputManager = PlayerConfigurationManager.Instance.gameObject.GetComponent<PlayerInputManager>();
        if (InputManager) InputManager.DisableJoining();
    }
}
