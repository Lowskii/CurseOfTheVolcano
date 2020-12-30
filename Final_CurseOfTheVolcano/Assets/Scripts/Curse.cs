using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurseType { Paralyse, SpeedDown, InverseControls, Bounce }
public class Curse : MonoBehaviour
{
    private CurseType m_CurrentCurseType;
    private int m_RandomNumer; 

    private ArrayList myList = new ArrayList();
    public Material[] myMaterials = new Material[4];

    public AudioSource m_AudioSource;

    private bool m_IsWorking;
    private float m__ElapsedTime=0;
    private float m__RunTime;

    [SerializeField] float m__RespawnTime;

    private void Start()
    {
        CreateRandomCurse();
    }
    private void CreateRandomCurse()
    {
        m_RandomNumer = Random.Range(0, 3);

        switch (m_RandomNumer)
        {
            case 0:
                m_CurrentCurseType = CurseType.Bounce;
                GetComponent<Renderer>().material = myMaterials[0];
                break;
            case 1:
                m_CurrentCurseType = CurseType.InverseControls;
                GetComponent<Renderer>().material = myMaterials[1];
                break;
            case 2:
                m_CurrentCurseType = CurseType.Paralyse; 
                GetComponent<Renderer>().material = myMaterials[2];
                break;
            case 3:
                m_CurrentCurseType = CurseType.SpeedDown;
                GetComponent<Renderer>().material = myMaterials[3];
                break;
            default:
                break;
        }

        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<SphereCollider>().enabled = true;
    }
    private void Update()
    {
        CheckIfTimeRunsOut();
    }
    private void CheckIfTimeRunsOut()
    {

        if (m_IsWorking)
        {
            m__ElapsedTime += Time.deltaTime;

            if (m__RunTime < m__ElapsedTime)
            {
                DeactivateCurseEffect();

            }else if (m__RespawnTime < m__ElapsedTime)
            {
                CreateRandomCurse();
                m_IsWorking = false;
                m__ElapsedTime = 0;

            }

        }
        
    }

    private void DeactivateCurseEffect()
    {
        if (m_CurrentCurseType == CurseType.SpeedDown)
        {
            foreach (GameObject item in myList)
            {
                if (item != null) NormalizeSpeedDownPlayers(item);
            }
        }
        else if (m_CurrentCurseType == CurseType.InverseControls)
        {
            foreach (GameObject item in myList)
            {
                if (item != null) NormalizeControlsPlayers(item);
            }
        }
        else if (m_CurrentCurseType == CurseType.Paralyse)
        {
            foreach (GameObject item in myList)
            {
                if (item != null) DeParalysePlayers(item);
            }
        }
        else if (m_CurrentCurseType == CurseType.Bounce)
        {
            foreach (GameObject item in myList)
            {
                if (item != null) LetPlayersStopBouncing(item);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/*other.gameObject.layer == LayerMask.GetMask("Player")*/other.gameObject.tag == "Player")
        {

            m_AudioSource.Play();

            FindAllEffectedPlayers(other);
            DeactivateVisuals();

            if (m_CurrentCurseType == CurseType.SpeedDown)
            {
                foreach (GameObject item in myList)
                {
                    if (item != null) SpeedDownPlayers(item);
                }
                m_IsWorking = true;

            }
            else if (m_CurrentCurseType == CurseType.InverseControls)
            {
                foreach (GameObject item in myList)
                {
                    if (item != null) InverseControlsPlayers(item);
                }
                m_IsWorking = true;

            }
            else if (m_CurrentCurseType == CurseType.Paralyse)
            {
                foreach (GameObject item in myList)
                {
                    if (item != null) ParalysePlayers(item);
                }
                m_IsWorking = true;

            }
            else if (m_CurrentCurseType == CurseType.Bounce)
            {
                foreach (GameObject item in myList)
                {
                    if (item != null) LetPlayersBounce(item);
                }
            }
            m_IsWorking = true;

        }
    }

    private void DeactivateVisuals()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;
    }

    private void LetPlayersStopBouncing(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_IsBouncing = false;
        }
    }

    private void DeParalysePlayers(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_Paralyse = false;
        }
    }

    private void NormalizeControlsPlayers(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_IsMovementInversed = false;
        }
    }

    private void NormalizeSpeedDownPlayers(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_IsSpeedDown = false;
        }
    }
    private void LetPlayersBounce(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_IsBouncing=true;
        }
    }

    private void ParalysePlayers(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_Paralyse = true;
        }
    }

    private void InverseControlsPlayers(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_IsMovementInversed = true;
        }
    }

    private void SpeedDownPlayers(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<CharacterControl>().m_IsSpeedDown = true;
        }
    }

    private void FindAllEffectedPlayers(Collider other)
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (object bject in allObjects)
        {
            if (myList.Contains(bject) == false)
            {
                myList.Add(bject);
            }
        }
        myList.Remove(other.gameObject);
    }
}
