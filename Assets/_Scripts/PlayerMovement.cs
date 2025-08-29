using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [Space]

    private Rigidbody rb;

    private Actions inputActions;

    [Space]
    [SerializeField] Transform orientation;

    //Get characters data from player data object
    private string characterName;

    //Movement
    [Header("Movement")]
    [Space]
    [SerializeField] float speedMultiplier;
    [SerializeField] float airSpeedMultiplier;
    private float walkSpeed;
    private float sprintSpeed;
    private float movementSpeed;
    private Vector2 movementInputDirection;
    private Vector3 movementDirection;

    //Jump
    private float jumpForce;


    //Ground Detection
    [Header("Ground Detection")]
    private bool playerGrounded;
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] Vector3 groundCheckHalfExtents;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayerMask;
    [Space]

    [Header("Drag Control")]
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    

    private void Awake()
    {
        inputActions = new();
    }

    private void OnEnable()
    {   
        inputActions.Player.Enable();

        inputActions.Player.Sprint.started += SpeedControl;
        inputActions.Player.Sprint.canceled += SpeedControl;

        inputActions.Player.Jump.started += Jump;

        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Sprint.started -= SpeedControl;
        inputActions.Player.Sprint.canceled -= SpeedControl;

        inputActions.Player.Jump.started -= Jump;
    }

    private void Start()
    {
        SetPlayerData();
    }

    private void SetPlayerData()
    {
        walkSpeed = playerData.walkSpeed;
        sprintSpeed = playerData.sprintSpeed;

        jumpForce = playerData.jumpForce;

        movementSpeed = walkSpeed;
    }

    private void Update()
    {
        CheckPlayerGrounded();
        Movement();
        DragControl();
    }

    private void CheckPlayerGrounded()
    {
        playerGrounded = Physics.CheckBox(groundCheckPosition.position + groundCheckOffset, groundCheckHalfExtents, Quaternion.identity, groundLayerMask);
    }

    private void Movement()
    {
        movementInputDirection = inputActions.Player.Movement.ReadValue<Vector2>();

        movementDirection = orientation.forward * movementInputDirection.y + orientation.right * movementInputDirection.x;
    }

    private void SpeedControl(InputAction.CallbackContext context)
    {
        if (!playerGrounded)
        {
            movementSpeed = walkSpeed;
        }
        else
        {
            if (context.started)
            {
                movementSpeed = sprintSpeed;
            }
            else
            {
                movementSpeed = walkSpeed;
            }
        }
    }

    private void DragControl()
    {
        if (playerGrounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = airDrag;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!playerGrounded) return;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (playerGrounded)
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * speedMultiplier, ForceMode.VelocityChange);    
        }
        else if (!playerGrounded)
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * speedMultiplier * airSpeedMultiplier * speedMultiplier, ForceMode.VelocityChange);
        }
    }

    private void OnDrawGizmos()
    {
        if (playerGrounded)
        {
            Gizmos.color = Color.green;
        }
        else 
        {
            Gizmos.color = Color.yellow;
        }
        Gizmos.DrawWireCube(groundCheckPosition.position + groundCheckOffset, groundCheckHalfExtents * 2);
    }
}
