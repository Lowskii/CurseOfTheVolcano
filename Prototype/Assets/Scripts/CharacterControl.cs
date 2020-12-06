using System.Collections;
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
    public bool _jump;

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

    private float SpeedUpTime, SpeedDownTime, ParalysedTime, DoubleJumpTime, InversedTime, BounceTime;

    private void Awake()
    {
        //spawning
        SpawnBeh = FindObjectOfType<SpawnBehaviour>();

       if(SpawnBeh) transform.position = SpawnBeh.GetSpawnPosition();

        //controls

        InputBeh = GetComponent<InputBehaviour>();

        InputBeh.StartJumpEvent.AddListener(StartJump);
        InputBeh.CancelJumpEvent.AddListener(CancelJump);        
    }
    
    public void StartJump()
    {
        _jump = true;
    }

    public void CancelJump()
    {
        _jump = false;
    }

    void Update()
    {
        _movementInput = InputBeh.RotationVector;

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
        if (IsAlwaysJumping && CC.isGrounded)
        {
            MoveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * BounceHeight);
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
            _velocity = MoveDirection.normalized * Speed;
        }
    }
}
