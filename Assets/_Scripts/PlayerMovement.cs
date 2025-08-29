using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    private Rigidbody rb;

    private Actions inputActions;

    //Get characters data from player data object
    private string characterName;

    //Movement
    private float walkSpeed;
    private float sprintSpeed;

    //Jump
    private float jumpForce;

    //Health
    private float maxHealth;

    //Local data

    //Movement
    private float movementSpeed;

    //Health
    private float currentHealth;

    //Ground Detection
    [Header("Ground Detection")]
    private bool playerGrounded;
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] Vector3 groundCheckHalfExtents;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayerMask;
    

    private void Awake()
    {
        inputActions = new();
    }

    private void OnEnable()
    {   
        inputActions.Player.Enable();

        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
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

        maxHealth = playerData.maxHealth;

        currentHealth = maxHealth;

        movementSpeed = walkSpeed;
    }

    private void Update()
    {
        CheckPlayerGrounded();
    }

    private void CheckPlayerGrounded()
    {
        playerGrounded = Physics.CheckBox(groundCheckPosition.position + groundCheckOffset, groundCheckHalfExtents, Quaternion.identity, groundLayerMask);
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
