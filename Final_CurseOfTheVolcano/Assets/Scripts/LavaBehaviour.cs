using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehaviour : MonoBehaviour
{
    [SerializeField] private float m_RiseSpeed;
    [SerializeField] private float m_MaxHeight;
    [SerializeField] private float m_RiseSpeedMultiplier = 0.25f;

    private float m_ActiveRiseSpeed;

    private void Start()
    {
        //CharacterControl.PlayerDiedEvent.AddListener(IncreaseSpeed);

        m_ActiveRiseSpeed = m_RiseSpeed;
    }

    private void Update()
    {
        if (transform.position.y <= m_MaxHeight)
        {
            transform.Translate(new Vector3(0, m_ActiveRiseSpeed * Time.deltaTime, 0));
        }
    }


    private void IncreaseSpeed()
    {
        m_ActiveRiseSpeed += m_RiseSpeed * m_RiseSpeedMultiplier;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            Destroy(hit.gameObject);

            //CharacterControl.PlayerDied();

            //TODO: Find alternative for ending it so abruptly 
            ////check if there is only one player left+
            //if (CharacterControl.PlayersAlive == 1)
            //{
            //    Application.Quit();
            //    UnityEditor.EditorApplication.isPlaying = false;
            //}
        }
    }
}
