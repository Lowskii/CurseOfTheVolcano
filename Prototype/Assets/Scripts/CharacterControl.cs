using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class CharacterControl : MonoBehaviour
{
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

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _movementInput;    
    private float _verticalInput, _horizontalInput,_turnSmoothVelocity;    
    private bool _jump;

    public float TrampolineForce = 15f;
    public float SpedUpSpeed = 1.5f;
    public float SlowedDownSpeed = 1.5f;
    public float TimeOutPickUpsAndCurses = 10;

    private bool IsSlowedDown = false;
    private bool IsSpedUp;
    private bool IsParalaysed = false;
    private bool IsAlwaysJumping = false;
    private bool IsDoubleJumpActive = false;
    private bool DoubleJumpPossible = false;
    private bool IsInverseControlActive = false;
    private bool TrampolineHit = false;

    private float SpeedUpTime, SpeedDownTime, ParalysedTime, DoubleJumpTime, InversedTime, BounceTime;

    private void Awake()
    {
        Controls = new Controls();
        Controls.PlayerControls.Jump.performed += context => _jump = true;
        Controls.PlayerControls.Jump.canceled += context => _jump = false;

        Controls.PlayerControls.Move.performed += context => _movementInput = context.ReadValue<Vector2>();
        Controls.PlayerControls.Move.canceled += context => _movementInput = Vector2.zero;
    }
    
    void Update()
    {        
        _verticalInput = _movementInput.y;
        _horizontalInput = _movementInput.x;        
        SetVelocity();        
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyJump();
        CheckControls();
        Bounce();
        CheckCoolDowns();
        CC.Move(_moveDirection * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        if (!CC.isGrounded)
        {
            _moveDirection.y += Physics.gravity.y * Mass * Time.fixedDeltaTime;           
        }

        if (CC.isGrounded)
        {
            _moveDirection.y = -CC.stepOffset * 10;           
        }        
    }
    private void BounceTimer()
    {
        if (IsAlwaysJumping)
        {
            BounceTime += Time.deltaTime;
            if (BounceTime > TimeOutPickUpsAndCurses)
            {
                IsAlwaysJumping = false;
            }
        }
    }
    private void ParalyseTimer()
    {
        if (IsParalaysed)
        {
            ParalysedTime += Time.deltaTime;
            if (ParalysedTime > TimeOutPickUpsAndCurses)
            {
                IsParalaysed = false;
            }
        }
    }
    private void DoubleJumpTimer()
    {
        if (IsDoubleJumpActive)
        {
            DoubleJumpTime += Time.deltaTime;
            if (DoubleJumpTime > TimeOutPickUpsAndCurses)
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
            if (SpeedUpTime > TimeOutPickUpsAndCurses)
            {
                IsSpedUp = false;
            }
        }
    }
    private void SpeedDownTimer()
    {
        if (IsSlowedDown)
        {
            SpeedDownTime += Time.deltaTime;
            if (SpeedDownTime > TimeOutPickUpsAndCurses)
            {
                IsSlowedDown = false;
            }
        }
    }
    private void InverseControlTimer()
    {
        if (IsInverseControlActive)
        {
            InversedTime += Time.deltaTime;
            if (InversedTime > TimeOutPickUpsAndCurses)
            {
                IsInverseControlActive = false;
            }
        }
    }

    private void CheckCoolDowns()
    {
        BounceTimer();
        ParalyseTimer();
        DoubleJumpTimer();
        SpeedDownTimer();
        SpeedUpTimer();
        InverseControlTimer();

    }
    private void Bounce()
    {
        if (TrampolineHit)
        {
            _moveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * TrampolineForce);
            TrampolineHit = false;
        }

        if (IsAlwaysJumping && CC.isGrounded)
        {
            _moveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * BounceHeight);
        }



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
            _moveDirection = Vector3.zero;
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
            _moveDirection.x = horizontal * Speed;
            _moveDirection.z = vertical * Speed;

            if (new Vector2(_moveDirection.x, _moveDirection.z).magnitude > 3)
            {
                float targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            }
        }

        if (!CC.isGrounded)
        {
            if (new Vector2(horizontal, vertical).magnitude > .1f)
            {
                float y = _moveDirection.y;
                _moveDirection = transform.forward * Speed;
                _moveDirection.y = y;
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
            _moveDirection.x = horizontal * Speed;
            _moveDirection.z = vertical * Speed;

            if (new Vector2(_moveDirection.x, _moveDirection.z).magnitude > 3)
            {
                float targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            }
        }

        if (!CC.isGrounded)
        {
            if (new Vector2(horizontal, vertical).magnitude > .1f)
            {
                float y = _moveDirection.y;
                _moveDirection = transform.forward * Speed;
                _moveDirection.y = y;
                float targetAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, JumpTurnSmoothTime);
                transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            }
        }
    }

    private void ApplyJump()
    {
        if (_jump && CC.isGrounded)
        {
            _jump = false;
            DoubleJumpPossible = true;

            _moveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);
        }

        else if (_jump && !CC.isGrounded && IsDoubleJumpActive && DoubleJumpPossible)
        {
            _jump = false;
            DoubleJumpPossible = false;
            _moveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);

        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PickUpSpeedUp"))
        {
            IsSpedUp = true;
            hit.gameObject.SetActive(false);
        }
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Trampoline"))
        {
            TrampolineHit = true;
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PickUpDoubleJump"))
        {
            IsDoubleJumpActive = true;
            hit.gameObject.SetActive(false);

        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CurseBounce"))
        {
            IsAlwaysJumping = true;
            hit.gameObject.SetActive(false);

        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CurseParalyse"))
        {
            IsParalaysed = true;
            hit.gameObject.SetActive(false);

        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CurseSpeedDown"))
        {
            IsSlowedDown = true;
            hit.gameObject.SetActive(false);

        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CurseInvertControls"))
        {
            IsInverseControlActive = true;
            hit.gameObject.SetActive(false);

        }
    }

    private void SetVelocity()
    {
        //Saves the player velocity before he jumps
        if(!_jump && CC.isGrounded)
        {
            _velocity = _moveDirection.normalized * Speed;
        }
    }

    private void OnEnable()
    {
        Controls.PlayerControls.Enable();
    }
    private void OnDisable()
    {
        Controls.PlayerControls.Disable();
    }
}
