using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.CharacterController))]
public class CharacterControl : MonoBehaviour
{
    public CharacterController m_CharacterController;

    [Range(0, 30)]
    public float m_MovementSpeed;

    [Range(0, 100)]
    public float NormalPushForce;
    [Range(0, 200)]
    public float StrongPushForce;
    private float CurrentPushForce;
    private float m_TurnSmoothVelocity;

    [Range(0, 5)]
    public float JumpHeight;

    [Tooltip("How smooth the character turns")]
    [Range(0, 1)]
    public float TurnSmoothTime;

    [Tooltip("How smooth the character turns while jumping")]
    [Range(0, 1)]
    public float JumpTurnSmoothTime;

    [Tooltip("Multiplies with gravity")]
    [Range(0, 100)]
    public float m_Mass;

    [SerializeField] private float m_Jumpspeed = 3.5f;

    public Vector3 m_MoveDirection = Vector3.zero;

    private bool m_IsPushPossible;
    public bool m_IsPushActivated;

    private float m_PushTimer,m_DelayTimer;
    private Vector3 m_Inpact = Vector3.zero;


    private float m_VerticalInput, m_HorizontalInput;
    public bool m_IsMovementInversed=true;
    public  bool m_IsSpeedDown=false;
    public bool m_IsBouncing = false;
    public bool m_Paralyse = false;


    private bool m_HasMoved = false;
    private bool m_IsDoubleJumpPossible = false;
    private bool m_IsDoubleJumpEnabled = false;
    public bool IsInteractPressed = false;    



    private void Update()
    {
        ApplyMovement();
        ApplyPush();
        StartDelayTimer();        
    }

    private void ApplyPush()
    {
        if (m_IsPushActivated)
        {
            m_PushTimer += Time.deltaTime;
            ApplyKNockBack();
            if (m_PushTimer >= 3 == false)
            {
                m_IsPushActivated = false;
                m_PushTimer = 0;
            }
        }
    }


    private void ApplyMovement()
    {
        if(!m_HasMoved) return;

        if (m_CharacterController.isGrounded)
        {
            if (m_MoveDirection.y < 0)
            {
                //reset y movement down when landing
                m_MoveDirection.y = -0.1f;
            }
            if (m_IsDoubleJumpPossible == false)
            {
                m_IsDoubleJumpPossible = true;
            }

        }
        else
        {
            //apply gravity
            m_MoveDirection.y += Physics.gravity.y * Time.deltaTime * m_Mass;
        }

        //rotation
        if (new Vector2(m_MoveDirection.x, m_MoveDirection.z).magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(m_MoveDirection.x, m_MoveDirection.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }

        Vector3 velocity = m_MoveDirection;

        if (m_IsMovementInversed)
        {
            velocity.x = -velocity.x;
            velocity.z = -velocity.z;
        }
        if (m_IsSpeedDown)
        {
            m_MovementSpeed = m_MovementSpeed / 2;
        }
        m_CharacterController.Move(velocity * Time.deltaTime * m_MovementSpeed);
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if (m_CharacterController.isGrounded || (m_IsDoubleJumpPossible && m_IsDoubleJumpEnabled))
        {
            m_MoveDirection.y = m_Jumpspeed;
            if (m_IsDoubleJumpPossible && !m_CharacterController.isGrounded)
            {
                m_IsDoubleJumpPossible = false;
            }
        }
    }

    public void Movement(InputAction.CallbackContext value)
    {
        m_HasMoved = true;

        var movement = value.ReadValue<Vector2>();

        m_HorizontalInput = movement.x;
        m_VerticalInput = movement.y;

        float yDir = m_MoveDirection.y;
        m_MoveDirection = new Vector3(m_HorizontalInput, 0, m_VerticalInput);

        m_MoveDirection.Normalize();
        m_MoveDirection.y = yDir;
    }
    public void Push(InputAction.CallbackContext value)
    {
        m_IsPushActivated = true;
        m_IsPushPossible = true;
    }

    public void KnockBack(Vector3 Direction, float Force)
    {
        Direction.Normalize();
        if (Direction.y < 0) Direction.y = -Direction.y; // reflect down force on the ground
        m_Inpact += Direction.normalized * Force / m_Mass;

    }
    public void ApplyKNockBack()
    {
        if (m_Inpact.magnitude > 0.2) m_CharacterController.Move(m_Inpact * Time.deltaTime * CurrentPushForce);
        // consumes the impact energy each cycle:
        m_Inpact = Vector3.Lerp(transform.position,transform.position + m_Inpact, 5 * Time.deltaTime);
    }

    public void Interact(InputAction.CallbackContext value)
    {
        IsInteractPressed = true;
    }

    private void StartDelayTimer()
    {       
        if (IsInteractPressed)
        {            
            float resetDelay = .1f;
            m_DelayTimer += Time.deltaTime;
            if (m_DelayTimer > resetDelay)
            {
                IsInteractPressed = false;
                m_DelayTimer -= resetDelay;
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player")&& m_IsPushPossible)
        {
            GameObject Player = other.gameObject;
            Player.GetComponent<CharacterControl>().KnockBack(m_MoveDirection, CurrentPushForce);
            Player.GetComponent<CharacterControl>().m_IsPushActivated = true;
            m_IsPushPossible = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player"))
        {
            m_IsPushPossible = false;

        }
    }
 
}
