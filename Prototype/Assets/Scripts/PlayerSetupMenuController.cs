using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    int _PlayerIndex;

    [SerializeField] private TextMeshProUGUI _TitleText;
    [SerializeField] private GameObject _ReadyPanel;
    [SerializeField] private GameObject _MenuPanel;
    [SerializeField] private Button _ReadyButton;

    private const float _IgnoreInputTime = 1.5f;
    private bool _InputEnabled = false;

    public void SetPlayerIndex(int index)
    {
        _PlayerIndex = index;
        _TitleText.SetText("Player " + (index + 1).ToString());

        Invoke("EnableInput", _IgnoreInputTime);
    }


    public void SetColor(Material color)
    {
        if(!_InputEnabled) { return; }


        PlayerConfigurationManager.Instance.SetPlayerColor(_PlayerIndex, color);
        _ReadyPanel.SetActive(true);
        _ReadyButton.Select();
        _MenuPanel.SetActive(false);
    }

    private void EnableInput()
    {
        _InputEnabled = true;
    }

    public void ReadyPlayer()
    {
        if (!_InputEnabled) { return; }

        _ReadyButton.gameObject.SetActive(false);
        PlayerConfigurationManager.Instance.ReadyPlayer(_PlayerIndex);
    }
}
