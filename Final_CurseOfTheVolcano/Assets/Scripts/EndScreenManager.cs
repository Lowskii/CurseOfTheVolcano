using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private Transform[] m_Positions = new Transform[4];
    [SerializeField] private GameObject[] m_UI = new GameObject[4];
    [SerializeField] private GameObject m_Model;    

    private List<Player> m_Players;    
    private LevelManager m_LevelManager;

    private void Awake()
    {
        m_LevelManager = FindObjectOfType<LevelManager>();
        m_Players = m_LevelManager.Players;       
        SetPlayerPosition();
    }

    private void SetPlayerPosition()
    {
        for (int i = 0; i < m_Players.Count; i++)
        {
            Vector3 position = m_Positions[i].position;
            Quaternion rotation = m_Positions[i].rotation;
            var player = Instantiate(m_Model, position, rotation);
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
