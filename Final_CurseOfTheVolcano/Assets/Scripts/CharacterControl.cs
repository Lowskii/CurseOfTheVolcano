using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(UnityEngine.CharacterController))]
public class CharacterControl : MonoBehaviour
{
    public CharacterController CC;
    public InputBehaviour Input;

    [Range(0, 30)]
    public float Speed;

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
    public float Mass;

    public Vector3 MoveDirection = Vector3.zero;
    public Vector3 _velocity = Vector3.zero;

    private Vector2 _movementInput;
    private float _verticalInput, _horizontalInput, _turnSmoothVelocity;
    private bool IsMovementInversed;
    private bool IsDoubleJUmpPossible;

    private void Start()
    {
        //controls
        Input =gameObject.GetComponent<InputBehaviour>();

    }

    private void Update()
    {
        SetVelocity();
        //CheckJump();
    }

    
    private void SetVelocity()
    {
        //Saves the player velocity before he jumps
        if (CC.isGrounded)
        {
            _velocity = MoveDirection.normalized * Speed;
        }
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyGravity();
        CC.Move(MoveDirection * Time.fixedDeltaTime);
    }

    private void CheckJump()
    {
        if (CC.isGrounded)
        {
            Jump();
        }
        else if (!CC.isGrounded && IsDoubleJUmpPossible)
        {
            Jump();
        }
    }
    private void Jump()
    {
        MoveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);
    }
    private void ApplyGravity()
    {
        if (!CC.isGrounded)
        {
            MoveDirection.y += Physics.gravity.y * Mass * Time.fixedDeltaTime;
        }

        if (CC.isGrounded)
        {
            MoveDirection.y = -CC.stepOffset * 10;
        }
    }
    private void ApplyMovement()
    {
        float horizontal = _horizontalInput;
        float vertical = _verticalInput;

        if (IsMovementInversed)
        {
            horizontal *= -1;
            vertical *= -1;
        }

        if (CC.isGrounded)
        {
            MoveDirection.x = horizontal * Speed;
            MoveDirection.z = vertical * Speed;

            if (new Vector2(MoveDirection.x, MoveDirection.z).magnitude > 3)
            {
                float targetAngle = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            }
        }

        if (!CC.isGrounded)
        {
            if (new Vector2(horizontal, vertical).magnitude > .1f)
            {
                float y = MoveDirection.y;
                MoveDirection = transform.forward * Speed;
                MoveDirection.y = y;
                float targetAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, JumpTurnSmoothTime);
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer==LayerMask.GetMask("Player"))
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
