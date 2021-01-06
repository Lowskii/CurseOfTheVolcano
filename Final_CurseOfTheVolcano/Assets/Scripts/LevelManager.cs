using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject LevelCanvas;
    public List<Player> Players = new List<Player>();
    private List<Player> m_LivePlayers = new List<Player>();
    public List<Player> LivePlayers => m_LivePlayers;
    private int m_PlayerCount = 0;

    [SerializeField] private Transform[] m_PlacementTransforms = new Transform[3];
    [SerializeField] private AudioSource m_AudioSource;
    private void Awake()
    {        
        DontDestroyOnLoad(this.gameObject);
        if (SceneManager.GetActiveScene().name == "L2_Kilimanjaro" || SceneManager.GetActiveScene().name == "L1_Vesuvius")
            m_PlayerCount = FindObjectsOfType<CharacterControl>().Length;
    }

    private void Update()
    {

        if (SceneManager.GetActiveScene().name == "L2_Kilimanjaro" || SceneManager.GetActiveScene().name == "L1_Vesuvius")
        {
            if (Players.Count == m_PlayerCount)
            {
                foreach (var player in FindObjectsOfType<CharacterControl>())
                {
                    player.GetComponent<InputBehaviour>().StopRumbleImmideately();
                    Destroy(player.gameObject);
                }
                foreach (var cam in FindObjectsOfType<Camera>())
                {
                    Destroy(cam.gameObject);
                }
                SceneManager.LoadScene("EndScreen");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Player newPlayer in m_LivePlayers)
        {
            if (newPlayer.PlayerID == other.GetComponent<CharacterControl>().PlayerId)
                return;
        }
        Player player = new Player(other.GetComponent<CharacterControl>().PlayerId, m_LivePlayers.Count + 1, other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.color, true);

        //Put the player on his placement position
        if (m_LivePlayers.Count < 3) //only 3 spots on tribune
        {
            other.transform.position = m_PlacementTransforms[m_LivePlayers.Count].position;
            other.transform.rotation = m_PlacementTransforms[m_LivePlayers.Count].rotation;
        }
        other.gameObject.GetComponent<Animator>().SetBool("HasWon", true);
        other.gameObject.transform.Find("Canvas").GetComponent<Animator>().SetTrigger("GameOver");
        other.gameObject.transform.Find("Canvas").transform.Find("Victory").gameObject.SetActive(true);
        other.GetComponent<CharacterControl>().enabled = false;

        m_LivePlayers.Add(player);
        Players.Add(player);
        
        //Avoids playing this sound when in endscreen.
        if (Players.Count < m_PlayerCount)
            m_AudioSource.Play();        
    }
}

public class Player
{
    public bool IsVictorious;
    public int PlayerID;
    public int VictoryPosition;
    public Color PlayerColor;

    public Player(int playerID, int victoryPosition, Color playerColor, bool isVictorious)
    {
        PlayerID = playerID;
        VictoryPosition = victoryPosition;
        IsVictorious = isVictorious;
        PlayerColor = playerColor;
    }
}
