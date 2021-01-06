using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.CharacterController))]
public class CharacterControl : MonoBehaviour
{
    private int m_PlayerId;

    public CharacterController CharacterController;

    [Range(0, 30)]
    public float MovementSpeed;

    [Range(0, 3)]
    public float NormalPushForce;
    [Range(0, 5)]
    public float StrongPushForce;
    private float m_CurrentPushForce;
    private float m_TurnSmoothVelocity;

    [SerializeField] private float m_TurnSmoothTime = 0.1f;
    [SerializeField] private GameObject m_SaveUI, m_PushUI, m_ExcamationUI;


    [Tooltip("Multiplies with gravity")]
    [Range(0, 100)]
    public float Mass;

    [SerializeField] private float m_Jumpspeed = 3.5f;

    private Vector3 m_MoveDirection = Vector3.zero;

    //pushing
    private bool m_IsPushActivated = false;
    private bool m_IsPushPossible = false;
    private float m_PushTimer = 0f;
    private float m_PushedSlowFactor = 3f;
    private float m_SlowSpeedFactor = 1.5f;
    private float m_SpeedupSpeedFactor = 1.5f;

    private float m_KnockBackTimer = 1f;
    [SerializeField] private float m_PushDelay = 2.5f;
    private bool m_GettingPushed = false;

    //saving
    private bool m_IsSaveJumpAvailable = false;
    private float m_SaveTime = 1.6f;

    private float m_DelayTimer;
    private Vector3 m_Inpact = Vector3.zero;
    private Vector3 m_CurrentInpact = Vector3.zero;

    private Vector3 m_ExternalVelocity = Vector3.zero;

    private bool m_JustJumped = false;
    public bool IsDoubleJumpEnabled = false;
    public bool IsSpedUp = false;
    public bool IsStrongerPush = false;


    public bool IsSpeedDown = false;
    public bool IsBouncing = false;
    public bool Paralyse = false;
    public bool IsMovementInversed = false;

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
        m_SaveUI.SetActive(false);
        m_CurrentPushForce = NormalPushForce;
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
        if (IsBouncing && CharacterController.isGrounded)
        {
            m_MoveDirection.y = m_Jumpspeed;
            m_JustJumped = true;
        }
    }

    private void CheckPushForce()
    {
        if (IsStrongerPush)
        {
            m_CurrentPushForce = StrongPushForce;
        }
        else
        {
            m_CurrentPushForce = NormalPushForce;

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

    public void AddExternalMovement(Vector3 velocity)
    {
        m_ExternalVelocity = velocity;
    }
    private void ApplyMovement()
    {

        if (CharacterController.isGrounded)
        {
            //keep a small motion down when the player is not jumping
            if (!m_JustJumped)
            {
                m_MoveDirection.y = -0.5f;
            }
            m_JustJumped = false;
            m_IsDoubleJumpPossible = true;
        }
        else
        {
            //apply gravity
            m_MoveDirection.y += Physics.gravity.y * Time.deltaTime * Mass;
        }

        if (!Paralyse) ApplyRotation(m_MoveDirection);

        Vector3 velocity = m_MoveDirection;

        //while getting pushed your movement is very limited
        if (m_GettingPushed)
        {
            velocity.x /= m_PushedSlowFactor;
            velocity.z /= m_PushedSlowFactor;
        }

        if (IsMovementInversed)
        {
            velocity.x = -velocity.x;
            velocity.z = -velocity.z;
        }

        if (IsSpeedDown)
        {
            velocity.x /= m_SlowSpeedFactor;
            velocity.z /= m_SlowSpeedFactor;
        }
        if (IsSpedUp)
        {
            velocity.x *= m_SpeedupSpeedFactor;
            velocity.z *= m_SpeedupSpeedFactor;
        }

        if (Paralyse)
        {
            velocity.x = 0;
            velocity.z = 0;

            if (velocity.y > 0)
            {
                velocity.y = 0;
            }
        }
        if (velocity.magnitude > 0.1f)
        {
            Vector3 movement = velocity * Time.deltaTime * MovementSpeed;

            if (m_ExternalVelocity.magnitude > 0.001f)
            {
                movement += m_ExternalVelocity;
                m_ExternalVelocity = Vector3.zero;
            }
            CharacterController.Move(movement);
        }
        //add external velocity
        else if (m_ExternalVelocity.magnitude > 0.1f)
        {
            CharacterController.Move(m_ExternalVelocity);
            m_ExternalVelocity = Vector3.zero;
        }


        float moveVelocity = new Vector3(velocity.x, 0, velocity.z).magnitude;
        m_Anim.SetFloat("Velocity", moveVelocity);
        m_Anim.SetBool("IsGrounded", CharacterController.isGrounded);
    }

    public void ApplyJumpForce(float jumpForce)
    {
        m_MoveDirection.y = jumpForce;
        m_JustJumped = true;
    }
    public void Jump(InputAction.CallbackContext value)
    {
        if (!this.enabled) return;

        if (CharacterController.isGrounded || (m_IsDoubleJumpPossible && IsDoubleJumpEnabled) || m_IsSaveJumpAvailable)
        {
            m_MoveDirection.y = m_Jumpspeed;
            m_JustJumped = true;

            if (!CharacterController.isGrounded)
            {
                m_SaveUI.SetActive(false);
                m_ExcamationUI.SetActive(false);
                m_IsSaveJumpAvailable = false;
            }
            if (m_IsDoubleJumpPossible && !CharacterController.isGrounded)
            {
                m_IsDoubleJumpPossible = false;
            }
        }
    }

    public void Movement(InputAction.CallbackContext value)
    {
        var movement = value.ReadValue<Vector2>();


        float yDir = m_MoveDirection.y;

        if (movement.magnitude > 0.1f)
        {
            m_MoveDirection = new Vector3(movement.x, 0, movement.y);
        }
        else
        {
            m_MoveDirection = new Vector3(0, 0, 0);
        }

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
        if (other.gameObject.tag == "Player" && m_IsPushPossible && !m_GettingPushed && other.gameObject != this.gameObject)
        {
            m_PushUI.SetActive(true);
        }
        if (other.gameObject.tag == "Player" && m_IsPushActivated && m_IsPushPossible && other.gameObject != this.gameObject)
        {
            m_IsPushPossible = false;
            GameObject player = other.gameObject;
            Vector3 dir = player.transform.position - transform.position;

            player.GetComponent<CharacterControl>().KnockBack(dir);
            player.GetComponent<InputBehaviour>().RumbleController(0.5f, 0.7f);            
        }
        m_IsPushActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && m_IsPushPossible && other.gameObject != this.gameObject)
        {
            m_PushUI.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        m_PushUI.SetActive(false);
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

        m_KnockBackTimer = 0.2f;
        m_GettingPushed = true;

        m_IsSaveJumpAvailable = true;
        m_PushUI.SetActive(false);
        m_SaveUI.SetActive(true);
        m_ExcamationUI.SetActive(true);
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
                CharacterController.Move(motion);
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
        m_SaveUI.SetActive(false);
        m_ExcamationUI.SetActive(false);
        m_IsSaveJumpAvailable = false;
    }
}
