using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private Transform[] m_Positions = new Transform[4];
    [SerializeField] private GameObject[] m_UI = new GameObject[4];
    [SerializeField] private GameObject m_Model;
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_VictoryMusic, m_DefeatMusic;
    private Controls m_GameInputControls;

    private List<Player> m_Players;    
    private LevelManager m_LevelManager;    

    private void Awake()
    {        
        m_GameInputControls = new Controls();
        m_GameInputControls.MenuControls.Enable();

        PlayerInput.ActionEvent startEvent = new PlayerInput.ActionEvent(m_GameInputControls.MenuControls.Start);

        m_GameInputControls.MenuControls.Start.performed += startEvent.Invoke;
        m_LevelManager = FindObjectOfType<LevelManager>();
        m_Players = m_LevelManager.Players;       
        SetPlayerPosition();

        if(m_LevelManager.LivePlayers.Count < 1)
            m_AudioSource.clip = m_DefeatMusic;        
        else
            m_AudioSource.clip = m_VictoryMusic;        
        m_AudioSource.Play();
    }

    private void SetPlayerPosition()
    {
        for (int i = 0; i < m_Players.Count; i++)
        {
            Vector3 position = m_Positions[i].position;
            Quaternion rotation = m_Positions[i].rotation;
            var player = Instantiate(m_Model, position, rotation);
            player.GetComponent<Animator>().SetBool("IsVictorious", m_Players[i].IsVictorious);
            foreach (var renderer in player.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.material.color = m_Players[i].PlayerColor;
            }
            Vector3 offset = new Vector3(0, 3, 0);
            if (m_Players[i].IsVictorious)
                Instantiate(m_UI[m_Players[i].VictoryPosition], player.transform.position+offset,Quaternion.identity);
            else
                Instantiate(m_UI[0], player.transform.position + offset, Quaternion.identity);
        }        
    }
}
