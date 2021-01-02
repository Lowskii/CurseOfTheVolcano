using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{      
    [SerializeField] private Image m_LoadingBar;
    private float m_CurrentValue;
    [HideInInspector] public float MaxValue;

    private void Awake()
    {        
        m_LoadingBar.fillAmount = m_CurrentValue/MaxValue;
    }

    private void Update()
    {
        m_LoadingBar.fillAmount = m_CurrentValue / MaxValue;
    }

    public IEnumerator StartUITimer()
    {
        m_CurrentValue = MaxValue;
        while (m_CurrentValue > 0)
        {
            m_CurrentValue -= .1f;
            yield return new WaitForSeconds(.1f);
        }        
    }
}
