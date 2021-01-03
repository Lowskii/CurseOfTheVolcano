using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelection : MonoBehaviour
{
    int m_PlayerIndex;
    int m_CurrentSelectedMaterial;
    bool m_IsReady = false;

    static List<Material> m_SelectedMaterials = new List<Material>();
    [SerializeField] private Material[] m_PlayerSelectionMaterials;
    [SerializeField] private SkinnedMeshRenderer m_MeshCharacter;

    [SerializeField] private TextMeshProUGUI m_TitleText;
    [SerializeField] private Camera m_Camera;

    [SerializeField] private GameObject m_ColorTakenWarning;
    [SerializeField] private GameObject m_ReadyPanel;
    [SerializeField] private GameObject m_SelectionPanel;

    [SerializeField] private Button m_CancelButton;
    [SerializeField] private Button m_ReadyButton;

    public bool IsReady
    { get { return m_IsReady; } }
    public void SetPlayerIndex(int index)
    {
        m_CurrentSelectedMaterial = m_PlayerIndex = index;
        m_MeshCharacter.GetComponentInParent<Animator>().SetBool("IsGrounded", true);
        m_TitleText.SetText("Player " + (index + 1).ToString());
    }

    static public void ResetCharacterSelection()
    {
        m_SelectedMaterials.Clear();
    }
    private void Start()
    {
        SetMaterial();

        m_ReadyPanel.SetActive(false);
    }

    void SetMaterial()
    {
        m_MeshCharacter.sharedMaterial = m_PlayerSelectionMaterials[m_CurrentSelectedMaterial];
        m_Camera.backgroundColor = m_PlayerSelectionMaterials[m_CurrentSelectedMaterial].color;
    }
    public void NextCharacter()
    {
        m_ColorTakenWarning.SetActive(false);

        do
        {
            ++m_CurrentSelectedMaterial;

            if (m_CurrentSelectedMaterial == m_PlayerSelectionMaterials.Length) m_CurrentSelectedMaterial = 0; //set back to first option
        }
        while (m_SelectedMaterials.Contains(m_PlayerSelectionMaterials[m_CurrentSelectedMaterial]));

        SetMaterial();
    }

    public void PreviousCharacter()
    {
        m_ColorTakenWarning.SetActive(false);

        do
        {
            --m_CurrentSelectedMaterial;

            if (m_CurrentSelectedMaterial < 0) m_CurrentSelectedMaterial = m_PlayerSelectionMaterials.Length - 1; //set to last option
        }
        while (m_SelectedMaterials.Contains(m_PlayerSelectionMaterials[m_CurrentSelectedMaterial]));

        SetMaterial();
    }

    public void ReadyUp()
    {
        //make sure no one else took this color before u
        if (m_SelectedMaterials.Contains(m_PlayerSelectionMaterials[m_CurrentSelectedMaterial]))
        {
            m_ColorTakenWarning.SetActive(true);

            Debug.Log("Color already taken!");
        }
        else
        {
            m_SelectedMaterials.Add(m_PlayerSelectionMaterials[m_CurrentSelectedMaterial]);
            m_ReadyPanel.SetActive(true);
            m_SelectionPanel.SetActive(false);

            m_CancelButton.Select();
            m_IsReady = true;
        }
    }

    public void CancelReadyUp()
    {
        m_SelectedMaterials.Remove(m_PlayerSelectionMaterials[m_CurrentSelectedMaterial]);

        m_ReadyPanel.SetActive(false);
        m_SelectionPanel.SetActive(true);

        m_ReadyButton.Select();
        m_IsReady = false;
    }
}
