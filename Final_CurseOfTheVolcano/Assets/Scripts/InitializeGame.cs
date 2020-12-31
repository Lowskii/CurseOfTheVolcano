using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    [SerializeField] Transform[] m_SpawnPoints = new Transform[4];

    private void Start()
    {
        InputBehaviour[] inputs = FindObjectsOfType<InputBehaviour>();


        for (int i = 0; i < inputs.Length; i++)
        {
            //reset mesh transform
            GameObject mesh = inputs[i].gameObject;
            mesh.transform.localScale = new Vector3(1, 1, 1);
            mesh.transform.localPosition = new Vector3(0, 0, 0);

            //set player to his new position
            inputs[i].transform.position = m_SpawnPoints[i].position;
            inputs[i].transform.rotation = m_SpawnPoints[i].rotation;

            //switch to game controls
            inputs[i].SwitchToGameActionMapping();

            //get the camera of the menu
            Camera menuCam = inputs[i].gameObject.transform.parent.GetComponentInChildren<Camera>();

            //make the player a seperate gameobject 
            inputs[i].transform.parent = null;

            //setup the camera rects 
            Camera playerCam = inputs[i].GetComponentInChildren<Camera>();
            playerCam.rect = menuCam.rect;
            playerCam.transform.parent = null;

            playerCam.GetComponent<CameraFollow>().enabled = true;

            //enable movement
            inputs[i].GetComponentInChildren<CharacterControl>().enabled = true;


        }
        //cleanup the unwanted objects
        Destroy(GameObject.Find("MainLayout"));
    }
}
