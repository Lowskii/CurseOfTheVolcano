using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurseType { Paralyse,SpeedDown,InverseControls,Bounce}
public class Curses : MonoBehaviour
{
    private ArrayList myList = new ArrayList();
    public GameObject Prefab ;
    public CurseType Curse;
    public float RunTime;
    private float _elapsedTime;
    private bool _isWorking;

    private void Update()
    {
        if (_isWorking)
        {
            InfluenceCurse();

            CheckIfTimeRunsOut();

        }

    }

    private void InfluenceCurse()
    {
        
        if (Curse == CurseType.InverseControls)
        {
            foreach (GameObject item in myList)
            {
                InverseControls(item);
            }
        }
        else if (Curse == CurseType.Paralyse)
        {
            foreach (GameObject item in myList)
            {
                Paralyse(item);
            }
        }
        else if (Curse == CurseType.SpeedDown)
        {
            foreach (GameObject item in myList)
            {
                SpeedDown(item);
            }
        }
        else if (Curse == CurseType.Bounce)
        {
            foreach (GameObject item in myList)
            {
                ActivateBounce(item);
            }
        }
    }

    private void ActivateBounce(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().SetBounceTrue();
        }
    }
    private void DeActivateBounce(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().SetBounceFalse();
        }
    }

    private void SpeedDown(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().SpeedDown();
        }
    }
    private void SpeedDownNormalize(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().NormalizeSpeedDown();
        }
    }

    private void Paralyse(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().Paralyse();
        }
    }
    private void DeParalyse(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().DeParalyse();
        }
    }

    private void InverseControls(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().SetControlsInverseActive();
        }

    }
    private void NormalizeControls(GameObject obj)
    {
        if (obj.GetComponent<CharacterController>() != null)
        {
            obj.GetComponent<CharacterControl>().SetControlsInverseNotActive();
        }

    }
    private void CheckIfTimeRunsOut()
    {
        if (_isWorking)
        {
            _elapsedTime += Time.deltaTime;

            if (RunTime < _elapsedTime)
            {
                _isWorking = false;

                if (Curse == CurseType.InverseControls)
                {
                    foreach (GameObject item in myList)
                    {
                        NormalizeControls(item);
                    }
                }
                else if (Curse == CurseType.Paralyse)
                {
                    foreach (GameObject item in myList)
                    {
                        DeParalyse(item);
                    }
                }
                else if (Curse == CurseType.SpeedDown)
                {
                    foreach (GameObject item in myList)
                    {
                        SpeedDownNormalize(item);
                    }
                }
                else if (Curse == CurseType.Bounce)
                {
                    foreach (GameObject item in myList)
                    {
                        DeActivateBounce(item);
                    }
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<SphereCollider>().enabled = false;
            this.GetComponentInChildren<TextMesh>().gameObject.active=false;


            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (object bject in allObjects)
            {
                if (myList.Contains(bject) == false)
                {
                    myList.Add(bject);
                }
            }
            myList.Remove(col.gameObject);
            _isWorking = true;
        }
    }

}
