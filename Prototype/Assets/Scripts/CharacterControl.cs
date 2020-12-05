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
        ApplyMovement();         
        ApplyJump();        
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

    private void ApplyMovement()
    {
        float horizontal = _horizontalInput;
        float vertical = _verticalInput;

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
            _moveDirection = _velocity;
            _moveDirection.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);
            
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
