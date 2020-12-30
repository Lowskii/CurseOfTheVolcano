using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PickUpType { SpedUp, DoubleJump, StrongerPush }

public class PickUp : MonoBehaviour
{
    private PickUpType m_PickUpType;
    private float RandomNumber;

    private float m__ElapsedTime=0;
    private float m__RunTime = 2;

    private GameObject Player;
    private bool m_IsWorking=false;

    [SerializeField] float m__RespawnTime;
    public Material[] myMaterials = new Material[3];
    public AudioSource m_AudioSource;


    private void Start()
    {
        CreateRandomPickUp();

    }

    private void Update()
    {
        CheckIfTimeRunsOut();
    }

    private void DeactivatePickUpEffect()
    {
        if (PickUpType.DoubleJump == m_PickUpType)
        {
     
            Player.GetComponent<CharacterControl>().m_IsDoubleJumpEnabled = false;

        }
        else if (PickUpType.SpedUp == m_PickUpType)
        {
            Player.GetComponent<CharacterControl>().m_IsSpedUp = false;

        }
        else if (PickUpType.StrongerPush == m_PickUpType)
        {
            Player.GetComponent<CharacterControl>().m_IsStrongerPush = false;

        }
    }
    private void CheckIfTimeRunsOut()
    {
        if (m_IsWorking)
        {

            m__ElapsedTime += Time.deltaTime;

            

            if (m__RespawnTime < m__ElapsedTime)
            {
                CreateRandomPickUp();
                m__ElapsedTime = 0;
                m_IsWorking = false;
            } else if (m__RunTime < m__ElapsedTime)
            {

                DeactivatePickUpEffect();
            }

        }
       
    }

    private void CreateRandomPickUp()
    {
        RandomNumber = Random.Range(0, 2);

        switch (RandomNumber)
        {
            case 0:
                m_PickUpType = PickUpType.SpedUp;
                GetComponent<Renderer>().material = myMaterials[0];
                break;
            case 1:
                m_PickUpType = PickUpType.DoubleJump;
                GetComponent<Renderer>().material = myMaterials[1];
                break;
            case 2:
                m_PickUpType = PickUpType.DoubleJump;
                GetComponent<Renderer>().material = myMaterials[2];
                break;
            default:
                break;
        }

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
                Player = other.gameObject;
                Player.GetComponent<CharacterControl>().m_IsDoubleJumpEnabled = true;

                m_IsWorking = true;
                DeactivateVisuals();
            }
            else if (PickUpType.SpedUp == m_PickUpType)
            {
                Player = other.gameObject;
                Player.GetComponent<CharacterControl>().m_IsSpedUp = true;

                m_IsWorking = true;
                DeactivateVisuals();

            }
            else if (PickUpType.StrongerPush == m_PickUpType)
            {
                Player = other.gameObject;
                Player.GetComponent<CharacterControl>().m_IsSpedUp = true;

                m_IsWorking = true;
                DeactivateVisuals();

            }
        //}
    }

    private void DeactivateVisuals()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;
    }
}
