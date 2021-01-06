using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class InitializeGame : MonoBehaviour
{
    [SerializeField] Transform[] m_SpawnPoints = new Transform[4];
    [SerializeField] LavaBehaviour m_LavaBeh;

    [SerializeField] List<GameObject> m_Players = new List<GameObject>();

    [SerializeField] GameObject m_IntroductionObjects;
    [SerializeField] GameObject m_ProgressBar;

    private const float m_AnimationStayDuration = 1.5f; //the time the camera stays at the end point of the dolly track

    [SerializeField] TextMeshProUGUI m_CountDownText;
    [SerializeField] float m_StartCountdown = 3.5f;
    bool m_IsCountdownActive = false;

    private void Start()
    {
        //add the skip function to the event
        foreach (PlayerInput.ActionEvent skipEvent in InputBehaviour.SkipEvents)
        {
            skipEvent.AddListener(Skip);
        }

        //invoke the start of the game countdown (when the intro animation is done)
        double startDuration = FindObjectOfType<PlayableDirector>().playableAsset.duration;
        Invoke("StartCountDown", (float)startDuration + m_AnimationStayDuration);

        InputBehaviour[] inputs = FindObjectsOfType<InputBehaviour>();

        for (int i = 0; i < inputs.Length; i++)
        {
            //reset mesh transform
            m_Players.Add(inputs[i].gameObject);
            m_Players[m_Players.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            m_Players[m_Players.Count - 1].transform.localPosition = new Vector3(0, 0, 0);

            //set player to his new position
            inputs[i].transform.position = m_SpawnPoints[i].position;
            inputs[i].transform.rotation = m_SpawnPoints[i].rotation;

            //switch to game controls
            inputs[i].SwitchToGameActionMapping();

            //get the camera of the menu
            Camera menuCam = inputs[i].gameObject.transform.parent.GetComponentInChildren<Camera>();

            //make the player a seperate gameobject 
            inputs[i].transform.parent = null;

            //add the player to the progress bar
            m_ProgressBar.GetComponent<LevelProgressionBar>().AddPlayer(inputs[i].gameObject);

             //setup the camera rects 
             Camera playerCam = inputs[i].GetComponentInChildren<Camera>();
            playerCam.rect = menuCam.rect;
            playerCam.transform.parent = null;

            playerCam.GetComponent<CameraFollow>().enabled = true;
        }
        //cleanup the unwanted objects
        Destroy(GameObject.Find("MainLayout"));
    }

    private void Skip(UnityEngine.InputSystem.InputAction.CallbackContext value)
    {
        StartCountDown();
        CancelInvoke("StartCountDown");
    }
    private void StartCountDown()
    {
        Destroy(m_IntroductionObjects);

        m_IsCountdownActive = true;

        m_ProgressBar.SetActive(true);
        m_ProgressBar.GetComponent<LevelProgressionBar>().EnableSliders();
    }
    private void StartGame()
    {
        m_LavaBeh.enabled = true;
        InputBehaviour.ResetSkipEvents();

        //enable movement
        foreach (GameObject player in m_Players)
        {
            player.GetComponentInChildren<CharacterControl>().enabled = true;
        }
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (m_IsCountdownActive)
        {
            m_StartCountdown -= Time.deltaTime;
            m_CountDownText.text = m_StartCountdown.ToString("0");

            if (m_StartCountdown <= 0) StartGame();
        }
    }
}

