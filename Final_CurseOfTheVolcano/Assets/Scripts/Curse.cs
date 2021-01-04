using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurseType { Paralyse, SpeedDown, InverseControls, Bounce }
public class Curse : MonoBehaviour
{
    private CurseType m_CurrentCurseType;
    private int m_RandomNumer;

    private ArrayList m_CurrentPlayerList = new ArrayList();
    public Material[] m_ListOfMaterials = new Material[4];

    public AudioSource m_AudioSource;

    [SerializeField] private float m_RunTimeBounce = 5;
    [SerializeField] private float m_RunTimeInverseControl = 3;
    [SerializeField] private float m_RunTimeParalyse = 10;
    [SerializeField] private float m_RunTimeSpeedDown = 5;

    private float m_RunTime = 10;

    [SerializeField] float m_RespawnTime;
    [SerializeField] private GameObject m_UILoader;
    [SerializeField] private Sprite m_Bounce, m_Stun, m_Speed,m_Inverse;
    private float m_logoTime=1.5f;

    private void Start()
    {
        CreateRandomCurse();
    }
    private void CreateRandomCurse()
    {
        m_RandomNumer = Random.Range(0, 4);

        switch (m_RandomNumer)
        {
            case 0:
                m_CurrentCurseType = CurseType.Bounce;
                GetComponent<Renderer>().material = m_ListOfMaterials[0];
                m_RunTime = m_RunTimeBounce;
                break;
            case 1:
                m_CurrentCurseType = CurseType.InverseControls;
                GetComponent<Renderer>().material = m_ListOfMaterials[1];
                m_RunTime = m_RunTimeInverseControl;
                break;
            case 2:
                m_CurrentCurseType = CurseType.Paralyse;
                GetComponent<Renderer>().material = m_ListOfMaterials[2];
                m_RunTime = m_RunTimeParalyse;
                break;
            case 3:
                m_CurrentCurseType = CurseType.SpeedDown;
                GetComponent<Renderer>().material = m_ListOfMaterials[3];
                m_RunTime = m_RunTimeSpeedDown;
                break;
            default:
                break;
        }

        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<SphereCollider>().enabled = true;
    }


    private IEnumerator TimeCurse()
    {
      
        yield return new WaitForSeconds(m_RunTime);
        DeactivateCurseEffect();

        yield return new WaitForSeconds(m_RespawnTime);
        CreateRandomCurse();
    }


    private void DeactivateCurseEffect()
    {
        if (m_CurrentCurseType == CurseType.SpeedDown)
        {
            foreach (GameObject item in m_CurrentPlayerList)
            {
                if (item != null) NormalizeSpeedDownPlayers(item);
                Destroy(item.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject);
            }
        }
        else if (m_CurrentCurseType == CurseType.InverseControls)
        {
            foreach (GameObject item in m_CurrentPlayerList)
            {
                if (item != null) NormalizeControlsPlayers(item);
                Destroy(item.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject);
            }
        }
        else if (m_CurrentCurseType == CurseType.Paralyse)
        {
            foreach (GameObject item in m_CurrentPlayerList)
            {
                if (item != null) DeParalysePlayers(item);
                Destroy(item.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject);
                item.GetComponent<Animator>().SetBool("IsStunned", false);
                item.transform.Find("StunParticles").gameObject.SetActive(false);
            }
        }
        else if (m_CurrentCurseType == CurseType.Bounce)
        {
            foreach (GameObject item in m_CurrentPlayerList)
            {
                if (item != null) LetPlayersStopBouncing(item);
                Destroy(item.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/*other.gameObject.layer == LayerMask.GetMask("Player")*/other.gameObject.tag == "Player")
        {
            FindObjectOfType<LevelManager>().LevelCanvas.GetComponent<Animator>().SetTrigger("ActivateCurse");
            m_AudioSource.Play();

            FindAllEffectedPlayers(other);
            DeactivateVisuals();

            if (m_CurrentCurseType == CurseType.SpeedDown)
            {
                foreach (GameObject item in m_CurrentPlayerList)
                {
                    ActivateSpeedDownCurse(item);
                }

            }
            else if (m_CurrentCurseType == CurseType.InverseControls)
            {
                Invoke("ActivateInverseControls",m_logoTime);
                

            }
            else if (m_CurrentCurseType == CurseType.Paralyse)
            {
                ActivateParalyseCurse();
            }
            else if (m_CurrentCurseType == CurseType.Bounce)
            {
                foreach (GameObject item in m_CurrentPlayerList)
                {
                    ActivateBounceCurse(item);
                }
            }
            StartCoroutine(TimeCurse());
        }
    }

    private void ActivateBounceCurse(GameObject item)
    {
        if (item != null) LetPlayersBounce(item);
        GridLayoutGroup grid = item.GetComponentInChildren<GridLayoutGroup>();
        FindObjectOfType<LevelManager>().LevelCanvas.GetComponentInChildren<Text>().text = "Bounce";
        GameObject loader = Instantiate(m_UILoader, grid.transform);
        loader.GetComponent<Image>().GetComponent<Loader>().MaxValue = m_RunTimeBounce;
        loader.GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
        loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().sprite = m_Bounce;
        loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
        StartCoroutine(loader.GetComponent<Image>().GetComponent<Loader>().StartUITimer());
    }

    private void ActivateSpeedDownCurse(GameObject item)
    {
        if (item != null) SpeedDownPlayers(item);
        GridLayoutGroup grid = item.GetComponentInChildren<GridLayoutGroup>();
        FindObjectOfType<LevelManager>().LevelCanvas.GetComponentInChildren<Text>().text = "Speed Down";
        GameObject loader = Instantiate(m_UILoader, grid.transform);
        loader.GetComponent<Image>().GetComponent<Loader>().MaxValue = m_RunTimeSpeedDown;
        loader.GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
        loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().sprite = m_Speed;
        loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
        StartCoroutine(loader.GetComponent<Image>().GetComponent<Loader>().StartUITimer());
    }

    private void ActivateParalyseCurse()
    {
        foreach (GameObject item in m_CurrentPlayerList)
        {
            if (item != null) ParalysePlayers(item);
            GridLayoutGroup grid = item.GetComponentInChildren<GridLayoutGroup>();
            FindObjectOfType<LevelManager>().LevelCanvas.GetComponentInChildren<Text>().text = "Stun";
            GameObject loader = Instantiate(m_UILoader, grid.transform);
            loader.GetComponent<Image>().GetComponent<Loader>().MaxValue = m_RunTimeParalyse;
            loader.GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().sprite = m_Stun;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(loader.GetComponent<Image>().GetComponent<Loader>().StartUITimer());
            item.GetComponent<Animator>().SetBool("IsStunned", true);
            item.transform.Find("StunParticles").gameObject.SetActive(true);
        }
    }

    private void ActivateInverseControls()
    {
        foreach (GameObject item in m_CurrentPlayerList)
        {
            if (item != null) InverseControlsPlayers(item);
            GridLayoutGroup grid = item.GetComponentInChildren<GridLayoutGroup>();
            FindObjectOfType<LevelManager>().LevelCanvas.GetComponentInChildren<Text>().text = "Inverse";
            GameObject loader = Instantiate(m_UILoader, grid.transform);
            loader.GetComponent<Image>().GetComponent<Loader>().MaxValue = m_RunTimeInverseControl;
            loader.GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().sprite = m_Inverse;
            loader.GetComponent<Image>().transform.Find("Art").GetComponent<Image>().color = this.gameObject.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(loader.GetComponent<Image>().GetComponent<Loader>().StartUITimer());
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
            item.GetComponent<CharacterControl>().m_IsBouncing = true;
        }
    }

    private void ParalysePlayers(GameObject item)
    {
        if (item.GetComponent<CharacterController>() != null)
        {
            item.GetComponent<InputBehaviour>().RumbleController(0.5f, m_RunTime);
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
            if (m_CurrentPlayerList.Contains(bject) == false)
            {
                m_CurrentPlayerList.Add(bject);
            }
        }
        m_CurrentPlayerList.Remove(other.gameObject);
    }
}
