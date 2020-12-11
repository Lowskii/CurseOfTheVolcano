using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> _PlayerConfigs;
    int _MaxPlayers = 4;
    int _MinPlayers = 2;


    public static PlayerConfigurationManager Instance { get; private set; }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _PlayerConfigs;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            _PlayerConfigs = new List<PlayerConfiguration>();
        }
    }

    public void SetPlayerColor(int index, Material color)
    {
        _PlayerConfigs[index].PlayerMaterial = color;
    }

    public void ReadyPlayer(int index)
    {
        _PlayerConfigs[index].IsReady = true;

        if (_PlayerConfigs.Count >= _MinPlayers && _PlayerConfigs.Count <= _MaxPlayers && _PlayerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("LevelPrototype");
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!_PlayerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            Debug.Log("Player " + pi.playerIndex + " joined");
            _PlayerConfigs.Add(new PlayerConfiguration(pi));
        }
        else
        {
            Debug.Log("Player " + pi.playerIndex + " reconnected");
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }

    public bool IsReady { get; set; }

    public Material PlayerMaterial { get; set; }
}
