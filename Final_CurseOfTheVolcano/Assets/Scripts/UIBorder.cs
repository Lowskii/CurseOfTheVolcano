using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBorder : MonoBehaviour
{
    [SerializeField] private Image m_Image;  
    private void Update()
    {
        Color color = this.gameObject.GetComponentInParent<CharacterControl>().gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        m_Image.color = color;
    }
}
