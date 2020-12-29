using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurseType { Paralyse, SpeedDown, InverseControls, Bounce }
public class Curse : MonoBehaviour
{
    public CurseType CurrentCurseType = CurseType.SpeedDown;

    private void Update()
    {
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player"))
        {
            if (CurrentCurseType==CurseType.SpeedDown)
            {
                GameObject Player = other.gameObject;
                Player.GetComponent<CharacterControl>().m_IsSpeedDown=true;
            }
            else if (CurrentCurseType == CurseType.Paralyse)
            {
                GameObject Player = other.gameObject;
                Player.GetComponent<CharacterControl>().m_Paralyse = true;
            }
            else if (CurrentCurseType == CurseType.InverseControls)
            {
                GameObject Player = other.gameObject;
                Player.GetComponent<CharacterControl>().m_IsMovementInversed = true;
            }
            else if (CurrentCurseType == CurseType.Bounce)
            {
                GameObject Player = other.gameObject;
                Player.GetComponent<CharacterControl>().m_IsBouncing = true;
            }
        }
    }
}
