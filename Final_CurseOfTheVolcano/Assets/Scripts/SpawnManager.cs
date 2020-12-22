using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject m_PlayerPrefab;
    [SerializeField] private Transform[] m_SpawnPoints = new Transform[4];

    private List<int> m_ControllerIds = new List<int>();


    private Controls m_GameInputControls;
    private int m_PlayerId = 0;
    private const int m_MaxPlayers = 4;

    void Update()
    {
        //check if start button is pressed
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            if (m_ControllerIds.Count == m_MaxPlayers)
            {
                Debug.Log("The maximum amount of players has already been reached.");
            }
            else
            {
                //check if controller is already used
                bool deviceIsUsed = false;

                if (m_ControllerIds.Contains(Gamepad.current.deviceId)) deviceIsUsed = true;


                if (deviceIsUsed)
                {
                    Debug.Log("Device is already assigned to a player");
                }
                //add a player
                else
                {
                    m_ControllerIds.Add(Gamepad.current.deviceId);

                    m_GameInputControls = new Controls();
                    m_GameInputControls.GameControls.Enable(); //we start in a menu

                    InputUser user = InputUser.PerformPairingWithDevice(Gamepad.current);
                    user.AssociateActionsWithUser(m_GameInputControls);

                    //spawn the player object and initiate it properly
                    GameObject player = Instantiate(m_PlayerPrefab);
                    player.transform.position = m_SpawnPoints[m_PlayerId].position;
                    player.transform.rotation = m_SpawnPoints[m_PlayerId].rotation;

                    InputBehaviour inputBeh = player.GetComponent<InputBehaviour>();                    
                    if (inputBeh != null) inputBeh.SetInputUser(user);

                    //
                    ++m_PlayerId;

                    Debug.Log("Player " + m_PlayerId + "joinend!");
                }
            }
        }
    }
}
