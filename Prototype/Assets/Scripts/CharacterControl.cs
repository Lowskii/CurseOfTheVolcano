﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class CharacterControl : MonoBehaviour
{
    InputBehaviour InputBeh;
    private SpawnBehaviour SpawnBeh;
    public CharacterController CC;
    public Controls Controls;
    public float JumpHeight;
    public float BounceHeight;

    [Range(0, 30)]
    public float Speed;    

    [Tooltip("How smooth the character turns")]
    [Range(0,1)]
    public float TurnSmoothTime;

    [Tooltip("How smooth the character turns while jumping")]
    [Range(0, 1)]
    public float JumpTurnSmoothTime;

    [Tooltip("Multiplies with gravity")]
    [Range(0,100)]
    public float Mass;

    public Vector3 MoveDirection = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _movementInput;    
    private float _verticalInput, _horizontalInput,_turnSmoothVelocity;    
    private bool _jump;
    public bool Interact;

    public float TrampolineForce = 15f;
    public float SpedUpSpeed = 1.5f;
    public float SlowedDownSpeed = 1.5f;


    public float RunOutTimeDoubleJump = 3;
    public float RunOutTimeSpeedUp = 5;


    private bool IsSlowedDown = false;
    private bool IsSpedUp;
    private bool IsParalaysed = false;
    private bool IsAlwaysJumping = false;
    private bool IsDoubleJumpActive = false;
    private bool DoubleJumpPossible = false;
    private bool IsInverseControlActive = false;   

    private float SpeedUpTime, SpeedDownTime, ParalysedTime, DoubleJumpTime, InversedTime, BounceTime;
    private bool _bounce;

    private void Awake()
    {
        //spawning
        SpawnBeh = FindObjectOfType<SpawnBehaviour>();

       if(SpawnBeh) transform.position = SpawnBeh.GetSpawnPosition();

        //controls

        InputBeh = GetComponent<InputBehaviour>();

        InputBeh.StartJumpEvent.AddListener(StartJump);
        InputBeh.CancelJumpEvent.AddListener(CancelJump);
        InputBeh.StartInteractEvent.AddListener(StartInteract);    
       
    }
    
    public void StartJump()
    {
        _jump = true;
    }
    public void StartInteract()
    {
        Interact = !Interact;       
    }    

    private void CancelJump()
    {
        _jump = false;
    }

    public void SetBounceTrue()
    {
        _bounce = true;
    }

    public void SetBounceFalse()
    {
        _bounce = false;
    }

    void Update()
    {
        _movementInput = InputBeh.RotationVector;
        Debug.Log(Interact);
        _verticalInput = _movementInput.y;
        _horizontalInput = _movementInput.x;        
        SetVelocity();        
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyJump();
        CheckControls();
        CheckCoolDowns();
        BounceWhenHitGround();
        CC.Move(MoveDirection * Time.fixedDeltaTime);
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
  
    private void DoubleJumpTimer()
    {
        if (IsDoubleJumpActive)
        {
            DoubleJumpTime += Time.deltaTime;
            if (DoubleJumpTime > RunOutTimeDoubleJump)
            {
                IsDoubleJumpActive = false;
            }
        }
    }
    private void SpeedUpTimer()
    {
        if (IsSpedUp)
        {
            SpeedUpTime += Time.deltaTime;
            if (SpeedUpTime > RunOutTimeSpeedUp)
            {
                IsSpedUp = false;
            }
        }
    }
 

    private void CheckCoolDowns()
    {
        
        DoubleJumpTimer();
        SpeedUpTimer();

    }
 

    private void CheckControls()
    {
        if (!IsParalaysed)
        {
            if (IsInverseControlActive == true)
            {
                ApplyInverseMovement();

            }
            else
            {
                ApplyMovement();
            }
        }
        else
        {
            MoveDirection = Vector3.zero;
        }
    }

    private void ApplyMovement()
    {
        float horizontal = _horizontalInput;
        float vertical = _verticalInput;

        if (IsSpedUp)
        {
            horizontal *= SpedUpSpeed;
            vertical *= SpedUpSpeed;
        }
        else if (IsSlowedDown)
        {
            horizontal /= SlowedDownSpeed;
            vertical /= SlowedDownSpeed;
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
    private void ApplyInverseMovement()
    {
        float horizontal = -1*_horizontalInput;
        float vertical = -1 * _verticalInput;

        if (IsSpedUp)
        {
            horizontal *= SpedUpSpeed;
            vertical *= SpedUpSpeed;
        }
        else if (IsSlowedDown)
        {
            horizontal /= SlowedDownSpeed;
            vertical /= SlowedDownSpeed;
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
    public void BounceWhenHitGround()
    {
        if (CC.isGrounded&&_bounce)
        {
            MoveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);
        }
    
    }
    public void SetControlsInverseActive()
    {
        IsInverseControlActive = true;
    }
    public void SetControlsInverseNotActive()
    {
        IsInverseControlActive = false;
    }

    public void Paralyse()
    {
        IsParalaysed = true;
    }
    public void DeParalyse()
    {
        IsParalaysed = false;
    }
    public void SpeedDown()
    {
        IsSlowedDown = true;
    }
    public void NormalizeSpeedDown()
    {
        IsSlowedDown = false;
    }
    public void ApplyJump()
    {
        if (_jump && CC.isGrounded)
        {
            _jump = false;
            DoubleJumpPossible = true;

            MoveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);
        }

        else if (_jump && !CC.isGrounded && IsDoubleJumpActive && DoubleJumpPossible)
        {
            _jump = false;
            DoubleJumpPossible = false;
            MoveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);
        }
    }
    

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
   
        //Debug.Log(hit.gameObject.name);
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PickUpSpeedUp"))
        {
            IsSpedUp = true;
            hit.gameObject.SetActive(false);
        }        
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PickUpDoubleJump"))
        {
            IsDoubleJumpActive = true;
            hit.gameObject.SetActive(false);

        }      
    }

    private void SetVelocity()
    {
        //Saves the player velocity before he jumps
        if(!_jump && CC.isGrounded)
        {
            _velocity = MoveDirection.normalized * Speed;
        }
    }
}
