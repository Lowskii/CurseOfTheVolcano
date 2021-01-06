using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum PickUpType { SpedUp, DoubleJump, StrongerPush }

public class PickUp : MonoBehaviour
{
    private PickUpType m_PickUpType;
    private float m_RandomNumber;
    private GameObject m_Player;
    [SerializeField] private float m_RunTimeDoubleJump = 5;
    [SerializeField] private float m_RunTimeSpeedUp = 3;
    [SerializeField] private float m_RunTimeStrongerPush = 10;
    [SerializeField] float m_RespawnTime;
    public AudioSource m_AudioSource;
    [SerializeField] private GameObject m_UILoader;
    [SerializeField] private Sprite m_Fist, m_DoubleJump, m_Speed;


    private void Start()
    {

        CreateRandomPickUp();

    }


    private IEnumerator CheckPickupTime(float runtime)
    {
        yield return new WaitForSeconds(runtime);
        DeactivatePickUpEffect();

        yield return new WaitForSeconds(m_RespawnTime);
        CreateRandomPickUp();
    }
    private void DeactivatePickUpEffect()
    {
        if (PickUpType.DoubleJump == m_PickUpType)
        {

            m_Player.GetComponent<CharacterControl>().m_IsDoubleJumpEnabled = false;
            Destroy(m_Player.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject);
        }
        else if (PickUpType.SpedUp == m_PickUpType)
        {
            m_Player.GetComponent<CharacterControl>().m_IsSpedUp = false;
            Destroy(m_Player.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject);
        }
        else if (PickUpType.StrongerPush == m_PickUpType)
        {
            m_Player.GetComponent<CharacterControl>().m_IsStrongerPush = false;
            Destroy(m_Player.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject);
        }
    }


    private void CreateRandomPickUp()
    {
        m_RandomNumber = Random.Range(0, 3);

        switch (m_RandomNumber)
        {
            case 0:
                m_PickUpType = PickUpType.SpedUp;
                break;
            case 1:
                m_PickUpType = PickUpType.DoubleJump;
                break;
            case 2:
                m_PickUpType = PickUpType.StrongerPush;
                break;
            default:
                break;
        }

        EnableVisuals();
    }

    private void EnableVisuals()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.GetMask("Player"))
        //{
        AudioSource.Play();

        if (PickUpType.DoubleJump == m_PickUpType)
        {
            m_Player = other.gameObject;
            m_Player.GetComponent<CharacterControl>().m_IsDoubleJumpEnabled = true;
            GridLayoutGroup grid = m_Player.GetComponentInChildren<GridLayoutGroup>();
            GameObject loader = Instantiate(m_UILoader, grid.transform);
            loader.GetComponent<Image>().GetComponent<Loader>().MaxValue = m_RunTimeDoubleJump;
            loader.GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().sprite = m_DoubleJump;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(loader.GetComponent<Image>().GetComponent<Loader>().StartUITimer());
            StartCoroutine(CheckPickupTime(m_RunTimeDoubleJump));

        }
        else if (PickUpType.SpedUp == m_PickUpType)
        {
            m_Player = other.gameObject;
            m_Player.GetComponent<CharacterControl>().m_IsSpedUp = true;
            GridLayoutGroup grid = m_Player.GetComponentInChildren<GridLayoutGroup>();
            GameObject loader = Instantiate(m_UILoader, grid.transform);
            loader.GetComponent<Image>().GetComponent<Loader>().MaxValue = m_RunTimeSpeedUp;
            loader.GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().sprite = m_Speed;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(loader.GetComponent<Image>().GetComponent<Loader>().StartUITimer());
            StartCoroutine(CheckPickupTime(m_RunTimeSpeedUp));
        }
        else if (PickUpType.StrongerPush == m_PickUpType)
        {
            m_Player = other.gameObject;
            m_Player.GetComponent<CharacterControl>().m_IsStrongerPush = true;
            GridLayoutGroup grid = m_Player.GetComponentInChildren<GridLayoutGroup>();
            GameObject loader = Instantiate(m_UILoader, grid.transform);
            loader.GetComponent<Image>().GetComponent<Loader>().MaxValue = m_RunTimeStrongerPush;
            loader.GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().sprite = m_Fist;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(loader.GetComponent<Image>().GetComponent<Loader>().StartUITimer());
            StartCoroutine(CheckPickupTime(m_RunTimeStrongerPush));
        }
        DeactivateVisuals();
        //}
    }

    private void DeactivateVisuals()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;
    }
}
