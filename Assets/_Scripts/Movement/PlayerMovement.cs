using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    walking,
    jumping,
    running
}

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] private MovementState movementState;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float groundDrag;
    [SerializeField] public bool freezeRotation;

    [Space]

    [SerializeField] public float jumpForce;
    //[SerializeField] public float jumpCooldown;
    [SerializeField] public float airMultiplier;

    [SerializeField] public float downwardsForce = 9.8f;

    [SerializeField] public bool readyToJump;
    

    [Header("Keybinds")]

    [SerializeField] public KeyCode jumpKey = KeyCode.Space;

    [Space]

    // ground check
    [SerializeField] float _playerHeight;
    [SerializeField] LayerMask _groundMasks;
    [SerializeField] bool _isGrounded;

    // members
    float _horizontalInput;
    float _verticalInput;

    Vector3 _moveDirection;

    private Rigidbody _rb;

    public static PlayerMovement Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = freezeRotation;
    }

    public Rigidbody GetRigidbody()
    {
        return _rb;
    }

    private void MovementInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

   void JumpInput()
    {
        if (Input.GetKey(jumpKey) && readyToJump && _isGrounded)
        {
            readyToJump = false;
            _isGrounded = false;

            Jump();
        }
    }

    private void FixedUpdate()
    {
        // Input handlers
        MovementInput();
        JumpInput();

        // Movement handlers
        MovePlayer();
        HandleAirborne();

        SpeedControl();

        // ground check
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f);

        if (_isGrounded)
        {
            _rb.drag = groundDrag;
            ResetJump();
        }
        else
        {
            _rb.drag = 0;
        }
    }

    void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (_isGrounded)
        {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void HandleAirborne()
    {
        if (_rb.velocity.y > 0f)
        {
            if (Input.GetKey(jumpKey))
            {
                _rb.AddForce(Vector3.down * downwardsForce / 2, ForceMode.Acceleration);
            }
            else
            {
                _rb.AddForce(Vector3.down * downwardsForce, ForceMode.Acceleration);
            }
        }
        else
        {
            _rb.AddForce(Vector3.down * downwardsForce, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
