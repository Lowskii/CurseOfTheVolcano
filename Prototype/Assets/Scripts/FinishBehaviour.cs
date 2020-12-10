using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBehaviour : MonoBehaviour
{
    private SpawnBehaviour SpawnBeh;

    private void Awake()
    {
        SpawnBeh = FindObjectOfType<SpawnBehaviour>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            Destroy(other.gameObject);

            if (SpawnBeh)
            {
                SpawnBeh.RemovePlayer();

                if (SpawnBeh.PlayersLeft == 1)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                    Application.Quit();
                }
            }
        }
    }
}