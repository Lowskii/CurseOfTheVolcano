using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject m_SelectionMenu;
    [SerializeField] GameObject m_MainLayout;

    [SerializeField] GameObject m_HelpText;


    private List<int> m_ControllerIds = new List<int>();
    private List<CharacterSelection> m_PlayerSelections = new List<CharacterSelection>();

    private Camera[] m_Cameras = new Camera[4];
    private Controls m_GameInputControls;
    private int m_PlayerId = 0;
    private const int m_MaxPlayers = 4;
    private const int m_MinPlayers = 2;

    private void Awake()
    {
        //we don't wanne destroy this since it will cary our players overs
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "EndScreen")
            DontDestroyOnLoad(m_MainLayout);


        //create controls so that we can start listening for the start command
        m_GameInputControls = new Controls();
        m_GameInputControls.MenuControls.Enable();

        PlayerInput.ActionEvent startEvent = new PlayerInput.ActionEvent(m_GameInputControls.MenuControls.Start);

        m_GameInputControls.MenuControls.Start.performed += startEvent.Invoke;

        startEvent.AddListener(PlayerPressedStart);
    }

    void PlayerPressedStart(InputAction.CallbackContext value)
    {
        //check for max amount of players
        if (m_ControllerIds.Count == m_MaxPlayers)
        {
            Debug.Log("The maximum amount of players has already been reached.");
        }
        else
        {
            //check if controller is already used
            if (m_ControllerIds.Contains(m_GameInputControls.MenuControls.Start.activeControl.device.deviceId))
            {
                Debug.Log("Device is already assigned to a player");
            }
            //add a player
            else
            {
                InputDevice controller = m_GameInputControls.MenuControls.Start.activeControl.device;
                m_ControllerIds.Add(controller.deviceId);

                //setup the controls and user
                Controls gameInputControls = new Controls();
                gameInputControls.MenuControls.Enable(); //we start in a menu

                InputUser user = InputUser.PerformPairingWithDevice(m_GameInputControls.MenuControls.Start.activeControl.device);
                user.AssociateActionsWithUser(gameInputControls);

                //spawn the player object and initiate it properly
                GameObject menu = Instantiate(m_SelectionMenu, m_MainLayout.transform);

                InputBehaviour inputBeh = menu.GetComponentInChildren<InputBehaviour>();
                if (inputBeh != null)
                {
                    inputBeh.SetInputUser(user, controller);
                    inputBeh.RumbleController(0.8f, 0.66f);
                }

                CharacterSelection characterSelection = menu.GetComponentInChildren<CharacterSelection>();
                if (characterSelection != null)
                {
                    characterSelection.SetPlayerIndex(m_PlayerId);
                    m_PlayerSelections.Add(characterSelection);
                }

                //set player id to the charactercontrol
                inputBeh.gameObject.GetComponent<CharacterControl>().PlayerId = m_PlayerId;


                //save the camera
                m_Cameras[m_PlayerId] = menu.GetComponentInChildren<Camera>();

                //inc playerid
                ++m_PlayerId;

                //this setup has to be after the increment (has to do with the actual player numbers and not their spot in an array which starts at 0)
                SetupCameras();

                Debug.Log("Player " + m_PlayerId + "joinend!");
            }
        }
        if (m_PlayerId == 4)
        {
            m_HelpText.SetActive(false);
        }
    }

    void SetupCameras()
    {
        //setup the cameras for the different splitscreen modes
        switch (m_PlayerId)
        {
            case 2:
                Rect playerCameraRect = m_Cameras[0].rect;
                playerCameraRect.width = 0.5f;
                playerCameraRect.height = 0.5f;
                playerCameraRect.x = 0.0f;
                playerCameraRect.y = 0.25f;
                m_Cameras[0].rect = playerCameraRect;

                playerCameraRect = m_Cameras[1].rect;
                playerCameraRect.width = 0.5f;
                playerCameraRect.height = 0.5f;
                playerCameraRect.x = 0.5f;
                playerCameraRect.y = 0.25f;
                m_Cameras[1].rect = playerCameraRect;
                break;
            case 3:
                playerCameraRect = m_Cameras[0].rect;
                playerCameraRect.height = 0.5f;
                playerCameraRect.width = 0.5f;
                playerCameraRect.y = 0.5f;
                playerCameraRect.x = 0f;
                m_Cameras[0].rect = playerCameraRect;

                playerCameraRect = m_Cameras[1].rect;
                playerCameraRect.x = 0.5f;
                playerCameraRect.y = 0.5f;
                playerCameraRect.width = 0.5f;
                playerCameraRect.height = 0.5f;
                m_Cameras[1].rect = playerCameraRect;

                playerCameraRect = m_Cameras[2].rect;
                playerCameraRect.x = 0.0f;
                playerCameraRect.y = 0.0f;
                playerCameraRect.width = 0.5f;
                playerCameraRect.height = 0.5f;
                m_Cameras[2].rect = playerCameraRect;
                break;
            case 4:
                playerCameraRect = m_Cameras[0].rect;
                playerCameraRect.height = 0.5f;
                playerCameraRect.width = 0.5f;
                playerCameraRect.y = 0.5f;
                playerCameraRect.x = 0.0f;
                m_Cameras[0].rect = playerCameraRect;

                playerCameraRect = m_Cameras[1].rect;
                playerCameraRect.x = 0.5f;
                playerCameraRect.y = 0.5f;
                playerCameraRect.width = 0.5f;
                playerCameraRect.height = 0.5f;
                m_Cameras[1].rect = playerCameraRect;

                playerCameraRect = m_Cameras[2].rect;
                playerCameraRect.x = 0.0f;
                playerCameraRect.y = 0.0f;
                playerCameraRect.width = 0.5f;
                playerCameraRect.height = 0.5f;
                m_Cameras[2].rect = playerCameraRect;

                playerCameraRect = m_Cameras[3].rect;
                playerCameraRect.x = 0.5f;
                playerCameraRect.y = 0.0f;
                playerCameraRect.width = 0.5f;
                playerCameraRect.height = 0.5f;
                m_Cameras[3].rect = playerCameraRect;
                break;
            default:
                break;
        }

    }

    private void Update()
    {
        //check if we are ready to start the game
        if (m_PlayerSelections.TrueForAll(p => p.IsReady) && m_PlayerSelections.Count > 1)
        {
            CharacterSelection.ResetCharacterSelection();

            if (MenuManager.m_LevelName.Length != 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(MenuManager.m_LevelName);
            }
            //hardcoded level load for easyer and faster testing during dev
            else
            {
                Debug.Log("remove for build");
                UnityEngine.SceneManagement.SceneManager.LoadScene("L2_Kilimanjaro");
            }
        }
    }
}
