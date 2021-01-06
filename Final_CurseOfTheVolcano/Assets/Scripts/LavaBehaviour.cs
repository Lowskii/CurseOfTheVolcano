using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaBehaviour : MonoBehaviour
{
    [SerializeField] private float m_RiseSpeed;
    [SerializeField] private float m_MaxHeight;
    [SerializeField] private float m_RiseSpeedMultiplier = 0.25f;

    [SerializeField] private AudioSource m_LavaSound;
    [SerializeField] private AudioSource m_ScreamSound;

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
    private IEnumerator PlaySound()
    {
        m_LavaSound.Play();
        yield return new WaitForSeconds(0.5f);
        m_ScreamSound.Play();
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            //Destroy(hit.gameObject);
            hit.gameObject.GetComponent<InputBehaviour>().RumbleController(0.8f, 1.5f);
            hit.gameObject.GetComponent<CharacterControl>().enabled = false;
            FindObjectOfType<LevelManager>().Players.Add(new Player(hit.GetComponent<CharacterControl>().PlayerId,
                0, hit.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.color, false));


            hit.gameObject.transform.Find("Canvas").GetComponent<Animator>().SetTrigger("GameOver");
            hit.gameObject.transform.Find("Canvas").transform.Find("Dead").gameObject.SetActive(true);
            hit.gameObject.transform.Find("Canvas").transform.Find("Red").gameObject.SetActive(true);
            hit.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

            FindObjectOfType<LevelProgressionBar>().PlayerDied(hit.gameObject);

            StartCoroutine(PlaySound());

            IncreaseSpeed();

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
