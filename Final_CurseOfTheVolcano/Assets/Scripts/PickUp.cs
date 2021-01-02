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
    public Material[] m_ListOfMaterials = new Material[3];
    public AudioSource m_AudioSource;
    [SerializeField] private GameObject m_Text;


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
        m_RandomNumber = Random.Range(0, 2);

        switch (m_RandomNumber)
        {
            case 0:
                m_PickUpType = PickUpType.SpedUp;
                GetComponent<Renderer>().material = m_ListOfMaterials[0];
                break;
            case 1:
                m_PickUpType = PickUpType.DoubleJump;
                GetComponent<Renderer>().material = m_ListOfMaterials[1];
                break;
            case 2:
                m_PickUpType = PickUpType.DoubleJump;
                GetComponent<Renderer>().material = m_ListOfMaterials[2];
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
        m_AudioSource.Play();

        if (PickUpType.DoubleJump == m_PickUpType)
        {
            m_Player = other.gameObject;
            m_Player.GetComponent<CharacterControl>().m_IsDoubleJumpEnabled = true;
            GridLayoutGroup grid = m_Player.GetComponentInChildren<GridLayoutGroup>();
            GameObject text = Instantiate(m_Text, grid.transform);
            text.GetComponent<Text>().text = "Double Jump";
            text.GetComponent<Text>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(CheckPickupTime(m_RunTimeDoubleJump));

        }
        else if (PickUpType.SpedUp == m_PickUpType)
        {
            m_Player = other.gameObject;
            m_Player.GetComponent<CharacterControl>().m_IsSpedUp = true;
            GridLayoutGroup grid = m_Player.GetComponentInChildren<GridLayoutGroup>();
            GameObject text = Instantiate(m_Text, grid.transform);
            text.GetComponent<Text>().text = "Speed Up";
            text.GetComponent<Text>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(CheckPickupTime(m_RunTimeSpeedUp));
        }
        else if (PickUpType.StrongerPush == m_PickUpType)
        {
            m_Player = other.gameObject;
            m_Player.GetComponent<CharacterControl>().m_IsSpedUp = true;
            GridLayoutGroup grid = m_Player.GetComponentInChildren<GridLayoutGroup>();
            GameObject text = Instantiate(m_Text, grid.transform);
            text.GetComponent<Text>().text = "Super Push";
            text.GetComponent<Text>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
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
