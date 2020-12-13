using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            Destroy(other.gameObject);

            CharacterControl.PlayerDied();

            if (CharacterControl.PlayersAlive == 1)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }
    }
}