using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressionBar : MonoBehaviour
{
    Dictionary<GameObject, Slider> m_PlayersSlider = new Dictionary<GameObject, Slider>();

    [SerializeField] private Slider m_Slider;
    [SerializeField] private Canvas m_Canvas;

    [SerializeField] private Sprite m_Skull;
    [SerializeField] private Sprite m_PlayerSprite;

    [SerializeField] private Transform m_Lava;

    [SerializeField] private Transform m_Finish;
    [SerializeField] private Transform m_StartPoint; //could be any of the players spawns since we only need the height

    private float m_DistanceToTop;

    private void Start()
    {
        m_DistanceToTop = m_Finish.position.y - m_StartPoint.position.y;
    }
    private void Update()
    {
        m_Slider.value = CalculateProgress(m_Lava.position.y);

        foreach (KeyValuePair<GameObject, Slider> playerSlider in m_PlayersSlider)
        {
            playerSlider.Value.value = CalculateProgress(playerSlider.Key.transform.position.y);
        }
    }

    public void AddPlayer(GameObject player)
    {
        Slider playerSlider = Instantiate(m_Slider, m_Canvas.transform);

        //disable the first to children because we don't need them
        playerSlider.transform.GetChild(0).gameObject.SetActive(false);
        playerSlider.transform.GetChild(1).gameObject.SetActive(false);

        //setup the handler sprite
        Image sliderImage = playerSlider.transform.GetChild(2).GetComponentInChildren<Image>();
        sliderImage.sprite = m_PlayerSprite;
        sliderImage.color = player.GetComponentInChildren<SkinnedMeshRenderer>().material.color;

        //disable till start of the game
        playerSlider.gameObject.SetActive(false);

        m_PlayersSlider.Add(player, playerSlider);
    }
    private float CalculateProgress(float yPos)
    {
        float currentPlayerDistance = m_Finish.position.y - yPos;
        float progress = 1.0f - (currentPlayerDistance / m_DistanceToTop);

        if (yPos > m_Finish.position.y) progress = 1f;
        else if (yPos < m_StartPoint.position.y) progress = 0f;

        return progress;
    }

    public void PlayerDied(GameObject player)
    {
        m_PlayersSlider[player].GetComponentInChildren<Image>().sprite = m_Skull;
    }

    public void EnableSliders()
    {
        foreach (KeyValuePair<GameObject, Slider> playerSlider in m_PlayersSlider)
        {
            playerSlider.Value.gameObject.SetActive(true);
        }
    }
}
