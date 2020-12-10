using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionScreenUi : MonoBehaviour
{
    private AsyncOperation sceneAsync;

    void Start()
    {
        Invoke("EndScene", 5f);
    }



    void EndScene()
    {

        AsyncOperation scene = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        scene.allowSceneActivation = false;
        sceneAsync = scene;

        //Wait until we are done loading the scene
        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + scene.progress);
        }

        //Activate the Scene
        sceneAsync.allowSceneActivation = true;


        Scene sceneToLoad = SceneManager.GetSceneByName("GameScene");
        if (sceneToLoad.IsValid())
        {
            Debug.Log("Scene is Valid");

            //get the players and move them
            var players = FindObjectsOfType<CharacterControl>();

            for (int i = 0; i < players.Length; i++)
            {
            SceneManager.MoveGameObjectToScene(players[i].gameObject, sceneToLoad);
            }


            //activate the scene
            SceneManager.SetActiveScene(sceneToLoad);
        }
        else
        {
            Debug.Log("Scene is NOT Valid");
        }
    }
}
