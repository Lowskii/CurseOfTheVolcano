using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PickUpType { SpedUp, DoubleJump, StrongerPush }

public class PickUp : MonoBehaviour
{
    private PickUpType m_PickUpType;
    private float m_RandomNumber;
    private GameObject m_Player;
    private float m__RunTime = 2;
    [SerializeField] float m__RespawnTime;
    public Material[] m_ListOfMaterials = new Material[3];
    public AudioSource m_AudioSource;


    private void Start()
    {
        CreateRandomPickUp();

    }

   
    private IEnumerator CheckPickupTime()
    {
        yield return new WaitForSeconds(m__RunTime);
        DeactivatePickUpEffect();

        yield return new WaitForSeconds(m__RespawnTime);
        CreateRandomPickUp();
    }
    private void DeactivatePickUpEffect()
    {
        if (PickUpType.DoubleJump == m_PickUpType)
        {
     
            m_Player.GetComponent<CharacterControl>().m_IsDoubleJumpEnabled = false;

        }
        else if (PickUpType.SpedUp == m_PickUpType)
        {
            m_Player.GetComponent<CharacterControl>().m_IsSpedUp = false;

        }
        else if (PickUpType.StrongerPush == m_PickUpType)
        {
            m_Player.GetComponent<CharacterControl>().m_IsStrongerPush = false;

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

        if (PickUpType.DoubleJump==m_PickUpType)
            {
                m_Player = other.gameObject;
                m_Player.GetComponent<CharacterControl>().m_IsDoubleJumpEnabled = true;

            }
        else if (PickUpType.SpedUp == m_PickUpType)
            {
                m_Player = other.gameObject;
                m_Player.GetComponent<CharacterControl>().m_IsSpedUp = true;


            }
        else if (PickUpType.StrongerPush == m_PickUpType)
            {
                m_Player = other.gameObject;
                m_Player.GetComponent<CharacterControl>().m_IsSpedUp = true;


            }
        DeactivateVisuals();
        CheckPickupTime();
        //}
    }

    private void DeactivateVisuals()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;
    }
}
