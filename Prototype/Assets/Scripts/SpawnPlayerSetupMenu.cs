using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    [SerializeField] GameObject _PlayerSetupMenuPrefab;
    public PlayerInput input;
    //public Camera Camera;
    private void Awake()
    {
        var rootMenu = GameObject.Find("MainLayout");

        if(rootMenu != null)
        {
            var menu = Instantiate(_PlayerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
            //Camera = this.gameObject.GetComponentInChildren<Camera>();
        }
    }
}
