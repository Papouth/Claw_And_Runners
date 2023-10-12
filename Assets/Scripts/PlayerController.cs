using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private InputManager inputManager;

    [SerializeField] private float speed = 8f;

    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform cameraCam;
    private float xRotation;
    [SerializeField] private float minClamp;
    [SerializeField] private float maxClamp;
    [SerializeField] private float mouseSensi = 20f;
    //-----------------//
    private CharacterController controller;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jump = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;
    #endregion


    #region Built In Methods
    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        CamMovement();
    }
    #endregion


    #region Customs Methods
    private void CamMovement()
    {
        var mouseX = inputManager.Look.x;
        var mouseY = inputManager.Look.y;

        cameraCam.position = cameraRoot.position;

        xRotation -= mouseY * mouseSensi * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, minClamp, maxClamp);

        cameraCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up, mouseX * mouseSensi * Time.deltaTime);
    }

    private void Move()
    {
        // GROUN CHECK
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // MOVEMENT
        float x = inputManager.Move.x;
        float z = inputManager.Move.y;
        
        Vector3 move = transform.right * x + transform.forward * z;
        
        controller.Move(move * speed * Time.deltaTime);

        // SAUT
        if (inputManager.CanJump && isGrounded)
        {
            inputManager.CanJump = false;
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    #endregion
}