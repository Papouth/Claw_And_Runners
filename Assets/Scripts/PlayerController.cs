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

    public float speed = 8f;

    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform cameraCam;
    [SerializeField] private Transform cameraCamDepth;

    [SerializeField] private GameObject[] playerSideArms;

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

    [SerializeField] private Transform ladderCheck;
    [SerializeField] private float ladderDistance;
    [SerializeField] private LayerMask ladderMask;
    private bool isLadder;

    [Tooltip("FootSteps")]
    [SerializeField] private float stepSize;
    private float timeStep;

    private Vector3 velocity;
    private bool isGrounded;

    private float oldForwardBackwardPosition;
    private float oldLeftRightPosition;
    private float oldUpDownPosition;

    [SerializeField] private NetworkVariable<float> forwardBackPosition = new NetworkVariable<float>();
    [SerializeField] private NetworkVariable<float> leftRightPosition = new NetworkVariable<float>();
    [SerializeField] private NetworkVariable<float> upDownPosition = new NetworkVariable<float>();

    private UIManager canvas;
    private TeamSelection teamSelection;
    private FootStepsSync footStepsSync;
    private PlayerInventory playerInventory;

    [SerializeField] private bool MathisDoitAnimer;
    #endregion


    #region Built In Methods
    private void Start()
    {
        // Canvas Settings
        canvas = FindObjectOfType<UIManager>();
        canvas.PanelOffOnStart();

        inputManager = GetComponent<InputManager>();
        controller = GetComponent<CharacterController>();

        footStepsSync = GetComponentInChildren<FootStepsSync>();

        playerInventory = GetComponent<PlayerInventory>();

        timeStep = stepSize;
    }

    public void MathisAnim()
    {
        MathisDoitAnimer = true;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            if (inputManager == null) inputManager = GetComponent<InputManager>();

            inputManager.enabled = false;

            cameraCam.GetComponent<Camera>().enabled = false;
            cameraCamDepth.GetComponent<Camera>().enabled = false;

            foreach (var item in playerSideArms)
            {
                item.SetActive(false);
            }

            teamSelection = FindObjectOfType<TeamSelection>();

            teamSelection.ShowHideUI();
        }
    }

    void Update()
    {
        if (IsServer && !IsOwner) UpdateServer();

        if (IsOwner) MoveClient();

        if (MathisDoitAnimer) MoveClient();
    }

    private void LateUpdate()
    {
        if (IsOwner) CamMovement();

        if (MathisDoitAnimer) CamMovement();
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

        // GROUND CHECK
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        isLadder = Physics.CheckSphere(ladderCheck.position, ladderDistance, ladderMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isLadder && velocity.y < 0) velocity.y = -2f;

        // MOVEMENT
        float x = inputManager.Move.x;
        float z = inputManager.Move.y;

        Vector3 move = transform.right * x + transform.forward * z;

        // SON FOOTSTEP
        if (move.magnitude != 0 && isGrounded)
        {
            // STEP SIZE TIMER
            if (timeStep > 0)
            {
                timeStep -= Time.deltaTime;
            }
            else if (timeStep <= 0)
            {
                footStepsSync.PlaySoundStep(Random.Range(0, footStepsSync.gameSounds.Length));
                timeStep = stepSize;
            }
        }
        else if (move.magnitude == 0 || !isGrounded)
        {
            timeStep = 0;
            footStepsSync.source.Stop();
        }


        // ANIMATION
        if (move.magnitude > 0.1 && isGrounded)
        {
            if (playerInventory.animatorsReady)
            {
                // Animation de course
                playerInventory.serverAnimator.SetInteger("IWR", 1);
                playerInventory.clientAnimator.SetInteger("IWR", 1);

                UpdateAnimServerRpc(1);
            }
        }
        else if (move.magnitude <= 0.1 && isGrounded)
        {
            if (playerInventory.animatorsReady)
            {
                // Animation d'Idle
                playerInventory.serverAnimator.SetInteger("IWR", 0);
                playerInventory.clientAnimator.SetInteger("IWR", 0);

                UpdateAnimServerRpc(0);
            }
        }

        controller.Move(move * speed * Time.deltaTime);

        // SAUT
        if (inputManager.CanJump && isGrounded)
        {
            // Animation 
            if (playerInventory.animatorsReady)
            {
                // Animation d'Idle
                playerInventory.serverAnimator.SetInteger("IWR", 5);
                playerInventory.clientAnimator.SetInteger("IWR", 5);

                UpdateAnimServerRpc(5);
            }

            inputManager.CanJump = false;
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }
        else if (inputManager.CanJump && !isGrounded)
        {
            //inputManager.CanJump = false;
        }

        // LADDER
        if (inputManager.CanJump && isLadder)
        {
            inputManager.CanJump = false;
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
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
    #endregion

    #region ServerRpc
    [ServerRpc]
    public void UpdateClientPositionServerRpc(float forwardBackward, float leftRight, float upDown)
    {
        forwardBackPosition.Value = forwardBackward;
        leftRightPosition.Value = leftRight;
        upDownPosition.Value = upDown;
    }

    [ServerRpc]
    public void UpdateAnimServerRpc(int animInt)
    {
        playerInventory.serverAnimator.SetInteger("IWR", animInt);
        playerInventory.clientAnimator.SetInteger("IWR", animInt);

        UpdateAnimClientRpc(animInt);
    }
    #endregion

    #region ClientRpc
    [ClientRpc]
    private void UpdateAnimClientRpc(int animInt)
    {
        playerInventory.clientAnimator.SetInteger("IWR", animInt);
    }
    #endregion
}