using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
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

    private float oldForwardBackwardPosition;
    private float oldLeftRightPosition;
    private float oldUpDownPosition;

    [SerializeField] private NetworkVariable<float> forwardBackPosition = new NetworkVariable<float>();
    [SerializeField] private NetworkVariable<float> leftRightPosition = new NetworkVariable<float>();
    [SerializeField] private NetworkVariable<float> upDownPosition = new NetworkVariable<float>();

    private UIManager canvas;
    #endregion


    #region Built In Methods
    private void Start()
    {
        // Canvas Settings
        canvas = FindObjectOfType<UIManager>();
        canvas.PanelOffOnStart();

        inputManager = GetComponent<InputManager>();
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(inputManager);
            cameraCam.GetComponent<Camera>().enabled = false;
        }
    }

    void Update()
    {
        if (IsServer && !IsOwner) UpdateServer();

        if (IsOwner) MoveClient();
    }

    private void LateUpdate()
    {
        if (IsOwner) CamMovement();
    }

    private void UpdateServer()
    {
        transform.position = new Vector3(transform.position.x + leftRightPosition.Value, transform.position.y + upDownPosition.Value, transform.position.z + forwardBackPosition.Value);
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

    private void MoveClient()
    {
        // SHARING INFO
        // float x -> left and right
        // float z -> forward and backward
        // velocity.y -> up and down

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
        else if (inputManager.CanJump && !isGrounded)
        {
            inputManager.CanJump = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (oldForwardBackwardPosition != z || oldLeftRightPosition != x || oldUpDownPosition != velocity.y)
        {
            oldForwardBackwardPosition = z;
            oldLeftRightPosition = x;
            oldUpDownPosition = velocity.y;

            // update the server
            UpdateClientPositionServerRpc(z, x, velocity.y);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float forwardBackward, float leftRight, float upDown)
    {
        forwardBackPosition.Value = forwardBackward;
        leftRightPosition.Value = leftRight;
        upDownPosition.Value = upDown;
    }
    #endregion
}