using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.CharacterController))]
public class CharacterControl : MonoBehaviour
{
    int m_PlayerId;

    public CharacterController m_CharacterController;

    [Range(0, 30)]
    public float m_MovementSpeed;

    [Range(0, 3)]
    public float m_NormalPushForce;
    [Range(0, 5)]
    public float m_StrongPushForce;
    private float m_CurrentPushForce;
    private float m_TurnSmoothVelocity;

    [SerializeField] float m_TurnSmoothTime = 0.1f;


    [Tooltip("Multiplies with gravity")]
    [Range(0, 100)]
    public float m_Mass;

    [SerializeField] private float m_Jumpspeed = 3.5f;

    public Vector3 m_MoveDirection = Vector3.zero;

    //pushing
    private bool m_IsPushActivated = false;
    private bool m_IsPushPossible = false;
    private float m_PushTimer = 0f;
    private float m_PushedSlowFactor = 3f;
    private float m_SlowSpeedFactor = 1.5f;
    private float m_SpeedupSpeedFactor = 1.5f;

    private float m_KnockBackTimer = 1f;
    [SerializeField] float m_PushDelay = 2.5f;
    bool m_GettingPushed = false;

    //saving
    bool m_IsSaveJumpAvailable = false;
    float m_SaveTime = 1.6f;

    private float m_DelayTimer;
    private Vector3 m_Inpact = Vector3.zero;
    private Vector3 m_CurrentInpact = Vector3.zero;


    private float m_VerticalInput, m_HorizontalInput;

    public bool m_IsDoubleJumpEnabled = false;
    public bool m_IsSpedUp = false;
    public bool m_IsStrongerPush = false;


    public bool m_IsSpeedDown = false;
    public bool m_IsBouncing = false;
    public bool m_Paralyse = false;
    public bool m_IsMovementInversed = false;

    private Animator m_Anim;

    private bool m_IsDoubleJumpPossible = false;
    public bool IsInteractPressed = false;

    public int PlayerId
    {
        get { return m_PlayerId; }
        set { m_PlayerId = value; }
    }

    private void Awake()
    {
        m_CurrentPushForce = m_NormalPushForce;
        m_Anim = this.gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        BounceWhenNeeded();
        CheckPushForce();
        ApplyMovement();
        ApplyPush();
        StartDelayTimer();
        ApplyKnockBack();        
    }

    private void BounceWhenNeeded()
    {
        if (m_IsBouncing && m_CharacterController.isGrounded)
        {
            m_MoveDirection.y = m_Jumpspeed;

        }
    }

    private void CheckPushForce()
    {
        if (m_IsStrongerPush)
        {
            m_CurrentPushForce = m_StrongPushForce;
        }
        else
        {
            m_CurrentPushForce = m_NormalPushForce;

        }
    }

    public void ApplyRotation(Vector3 direction)
    {
        if (new Vector2(direction.x, direction.z).magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, m_TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }
    }
    private void ApplyMovement()
    {   

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

        ApplyRotation(m_MoveDirection);

        Vector3 velocity = m_MoveDirection;        

        //while getting pushed your movement is very limited
        if (m_GettingPushed)
        {
            velocity.x /= m_PushedSlowFactor;
            velocity.z /= m_PushedSlowFactor;
        }

        if (m_IsMovementInversed)
        {
            velocity.x = -velocity.x;
            velocity.z = -velocity.z;
        }

        if (m_IsSpeedDown)
        {
            velocity.x /= m_SlowSpeedFactor;
            velocity.z /= m_SlowSpeedFactor;
        }
        if (m_IsSpedUp)
        {
            velocity.x *= m_SpeedupSpeedFactor;
            velocity.z *= m_SpeedupSpeedFactor;
        }

        if (velocity.magnitude > 0.1f)
        {            
            if (m_Paralyse)
            {
                m_CharacterController.Move(Vector3.zero);                
            }
            else
            {                
                m_CharacterController.Move(velocity * Time.deltaTime * m_MovementSpeed);
            }
        }
        m_Anim.SetFloat("Velocity", velocity.magnitude);
        m_Anim.SetBool("IsGrounded", m_CharacterController.isGrounded);
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if (m_CharacterController.isGrounded || (m_IsDoubleJumpPossible && m_IsDoubleJumpEnabled) || m_IsSaveJumpAvailable)
        {
            m_MoveDirection.y = m_Jumpspeed;           
            if (!m_CharacterController.isGrounded)
            {
                m_IsSaveJumpAvailable = false;
            }
            if (m_IsDoubleJumpPossible && !m_CharacterController.isGrounded)
            {
                m_IsDoubleJumpPossible = false;
            }
        }            
    }

    public void Movement(InputAction.CallbackContext value)
    {
        var movement = value.ReadValue<Vector2>();

        m_HorizontalInput = movement.x;
        m_VerticalInput = movement.y;

        float yDir = m_MoveDirection.y;
        m_MoveDirection = new Vector3(m_HorizontalInput, 0, m_VerticalInput);

        m_MoveDirection.Normalize();
        m_MoveDirection.y = yDir;
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
        if (other.gameObject == gameObject) return;

        if (other.gameObject.tag == "Player" && m_IsPushActivated && m_IsPushPossible)
        {
            m_IsPushPossible = false;

            GameObject Player = other.gameObject;
            Vector3 dir = Player.transform.position - transform.position;

            Player.GetComponent<CharacterControl>().KnockBack(dir);
        }
        m_IsPushActivated = false;
    }

    public void Push(InputAction.CallbackContext value)
    {
        m_IsPushActivated = true;

        Invoke("DisablePushActivated", 0.25f);
    }

    public void KnockBack(Vector3 direction)
    {
        direction.y = 0; //we don't want people getting knocked up or down
        direction.Normalize();

        m_Inpact = direction * m_CurrentPushForce;
        m_CurrentInpact = m_Inpact;

        m_KnockBackTimer = 0f;
        m_GettingPushed = true;

        m_IsSaveJumpAvailable = true;
        Invoke("DisableSaveJump", m_SaveTime);
    }
    public void ApplyKnockBack()
    {
        if (m_KnockBackTimer < 1f)
        {
            m_KnockBackTimer += Time.deltaTime;

            //get the new position of this frame and reduce the distance of where the player will be trown
            Vector3 destPos = Vector3.Lerp(transform.position, transform.position + m_CurrentInpact, m_KnockBackTimer);
            m_CurrentInpact = Vector3.Lerp(m_Inpact, Vector3.zero, m_KnockBackTimer);

            Vector3 motion = destPos - transform.position;

            if (motion.magnitude > 0.1f)
            {
                m_CharacterController.Move(motion);
            }
        }
        else
        {
            m_GettingPushed = false;
        }
    }

    private void ApplyPush()
    {
        if (!m_IsPushPossible)
        {
            m_PushTimer += Time.deltaTime;

            if (m_PushTimer >= m_PushDelay)
            {
                m_IsPushPossible = true;
                m_PushTimer = 0;
            }
        }
    }

    void DisablePushActivated()
    {
        m_IsPushActivated = false;
    }
    void DisableSaveJump()
    {
        m_IsSaveJumpAvailable = false;
    }
}
