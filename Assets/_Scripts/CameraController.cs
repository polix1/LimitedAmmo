using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //Inputs
    private Actions inputActions;

    //Position
    [SerializeField] Transform cameraPosition;

    //Camera Look
    [SerializeField] float sensX, sensY;
    [SerializeField] float sensMultiplier = 0.2f;
    private float rotX, rotY;
    private Vector2 mouseDelta;

    //orientation
    [SerializeField] Transform orientation;

    //player
    [SerializeField] Transform playerTransform;


    private void Awake()
    {
        inputActions = new();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Camera.Enable();
    }

    private void OnDisable()
    {
        inputActions.Camera.Disable();
    }

    private void LateUpdate()
    {
        transform.position = cameraPosition.position;
    }

    private void Update()
    {
        //Get mouse delta
        mouseDelta = inputActions.Camera.Look.ReadValue<Vector2>();

        //Adds the rotation based on the mouse delta
        rotX -= mouseDelta.y * sensX * sensMultiplier;
        rotY += mouseDelta.x * sensY * sensMultiplier;

        //Clamps the x axis
        rotX = Mathf.Clamp(rotX, -90, 90);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.rotation = Quaternion.Euler(0, rotY, 0);
        playerTransform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
