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

    private Vector3 m_MoveDirection = Vector3.zero;


    private float m_VerticalInput, m_HorizontalInput;
    private bool m_IsMovementInversed;
    private bool m_IsDoubleJumpPossible = false;
    private bool m_IsDoubleJumpEnabled = false;

    /*
     * clean up the code
     * rotation
     * camera
     */

    private void Update()
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

        Vector3 velocity = m_MoveDirection;

        if (m_IsMovementInversed)
        {
            velocity.x = -velocity.x;
            velocity.z = -velocity.z;
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

    }
    public void Interact(InputAction.CallbackContext value)
    {

    }
    private void ApplyGravity()
    {

    }


    private void ApplyMovement()
    {
        //float horizontal = m_HorizontalInput;
        //float vertical = m_VerticalInput;

        //if (IsMovementInversed)
        //{
        //    horizontal *= -1;
        //    vertical *= -1;
        //}

        //if (m_CharacterController.isGrounded)
        //{
        //    MoveDirection.x = horizontal * Speed;
        //    MoveDirection.z = vertical * Speed;

        //    if (new Vector2(MoveDirection.x, MoveDirection.z).magnitude > 3)
        //    {
        //        float targetAngle = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg;
        //        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
        //        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        //    }
        //}

        //if (!m_CharacterController.isGrounded)
        //{
        //    if (new Vector2(horizontal, vertical).magnitude > .1f)
        //    {
        //        float y = MoveDirection.y;
        //        MoveDirection = transform.forward * Speed;
        //        MoveDirection.y = y;
        //        float targetAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
        //        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, JumpTurnSmoothTime);
        //        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        //    }
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player"))
        {
            TriggerPush(other);
        }
    }

    private void TriggerPush(Collider col)
    {
        Vector3 dir = col.transform.position - transform.position;
        dir = dir.normalized;

        //Vector3.Lerp(transform.position, transform.position + dir);
        //other.transform.position = Vector3.Lerp(transform.position, transform.position - dir * CurrentForce, TurnSmoothTime);

        CharacterController CC = col.GetComponent<CharacterController>();
        CC.Move(dir * CurrentPushForce * Time.deltaTime);
    }
}
