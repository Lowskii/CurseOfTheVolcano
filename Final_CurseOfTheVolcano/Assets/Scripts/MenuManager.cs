using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string m_SceneName;
    public void PlayGame()
    {
        SceneManager.LoadScene(m_SceneName);
    }
    public void QuitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
    public void LoadControls()
    {

    }
}
