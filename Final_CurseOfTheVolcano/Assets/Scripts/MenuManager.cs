using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string m_SceneName;

    public static string m_LevelName;

    public void QuitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
    public void PlayLevel1()
    {
        m_LevelName = "L1_Vesuvius";

        SceneManager.LoadScene(m_SceneName);
    }

    public void PlayLevel2()
    {
        m_LevelName = "L2_Kilimanjaro";

        SceneManager.LoadScene(m_SceneName);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(m_SceneName);
        Destroy(FindObjectOfType<LevelManager>().gameObject);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("StartMenu");
        Destroy(FindObjectOfType<LevelManager>().gameObject);
    }
}
