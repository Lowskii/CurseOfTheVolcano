using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class CharacterControl : MonoBehaviour
{
    public CharacterController CC;
    public Controls Controls;

    [Tooltip("Units/seconds")]
    [Range(0,30)]
    public float Speed = 10;

    [Tooltip("How smooth the character turns")]
    [Range(0.0f,1f)]
    public float TurnSmoothTime = .1f;

    private Vector3 _velocity = Vector3.zero;
    private Vector2 _movementInput;
    private float _forwardInput, _horizontalInput,_turnSmoothVelocity;

    public float JumpHeight;
    private bool _jump;
    private void Awake()
    {
        Controls = new Controls();
        Controls.PlayerControls.Jump.performed += context => _jump = true;
        Controls.PlayerControls.Jump.canceled += context => _jump = false;

        Controls.PlayerControls.Move.performed += context => _movementInput = context.ReadValue<Vector2>();
        Controls.PlayerControls.Move.canceled += context => _movementInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {        
        _forwardInput = _movementInput.y;
        _horizontalInput = _movementInput.x;       
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyMovement();        

        if (CC.isGrounded)
        {
            _velocity.y = -CC.stepOffset * 10;
        }
        ApplyJump();
        CC.Move(_velocity * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        if (!CC.isGrounded)
        {
            _velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        }
       
    }   

    private void ApplyMovement()
    {
        float horizontal = _horizontalInput;
        float vertical = _forwardInput;
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if(direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            CC.Move(direction * Speed * Time.deltaTime);
        }
    }

    private void ApplyJump()
    {        
        if (_jump && CC.isGrounded)
        {
            _jump = false;
            _velocity.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * JumpHeight);
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
