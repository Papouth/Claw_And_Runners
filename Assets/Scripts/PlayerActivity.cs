using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerActivity : NetworkBehaviour
{
    #region Variables
    [Header("Settings")]
    private InputManager inputManager;
    private PlayerInventory playerInventory;
    private PlayerShoot playerShoot;
    private PlayerInfo PI;
    private GameManager GM;
    private CharacterController CC;

    private bool ccState;

    public bool inTrigger;
    public bool playerInActivity;

    [Header("Activity Bool")]
    [HideInInspector] public bool standTir;
    [HideInInspector] public bool piano;


    [Header("Stand de Tir")]
    [SerializeField] private GameObject pistolCopPrefab;
    [SerializeField] private GameObject pistolRunnerPrefab;
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
        GM = FindObjectOfType<GameManager>();

        CC = GetComponent<CharacterController>();
        ccState = true;

        inputManager = GetComponent<InputManager>();

        playerInventory = GetComponent<PlayerInventory>();

        playerShoot = GetComponentInChildren<PlayerShoot>();

        PI = GetComponent<PlayerInfo>();

        //pistolCopPrefab.SetActive(false);
        //pistolRunnerPrefab.SetActive(false);
    }

    private void Update()
    {
        InteractWithActivity();

        LeaveActivity();

        StaticController();
    }
    #endregion


    #region Customs Methods
    private void StaticController()
    {
        if (playerInActivity && piano && Input.GetKeyDown(KeyCode.LeftAlt) && IsOwner)
        {
            ccState = !ccState;
            CC.enabled = ccState;
        }
    }

    private void LeaveActivity()
    {
        if (!inTrigger && playerInActivity && standTir)
        {
            Debug.Log("Je quitte le stand de tir");

            // On désactive le crosshair
            GM.panelShootingRange.SetActive(false);

            // Bool player In Activity et standTir repassent en false
            playerInActivity = false;
            standTir = false;

            // On range le pistolet
            Pistol(false);

            // Fin animation pistolet
            playerShoot.playerAnimator.SetBool("PistolOn", false);

            // On enlève le format d'activité pour l'inventaire
            playerInventory.inActivity = false;
        }
        else if (!inTrigger && playerInActivity && piano)
        {
            Debug.Log("Je quitte le piano");

            // On retire UI du piano
            GM.UIM.panelPiano.SetActive(false);

            // Bool player In Activity et piano repassent en false
            playerInActivity = false;
            piano = false;

            // On enlève le format d'activité pour l'inventaire
            playerInventory.inActivity = false;
        }
    }

    private void ForceLeaveActivity()
    {
        Debug.Log("Je quitte l'activité");

        // On désactive le crosshair
        GM.panelShootingRange.SetActive(false);

        // On retire UI du piano
        GM.UIM.panelPiano.SetActive(false);

        // Bool player In Activity et standTir repassent en false
        playerInActivity = false;
        standTir = false;
        piano = false;

        // On range le pistolet
        Pistol(false);

        // Fin animation pistolet
        playerShoot.playerAnimator.SetBool("PistolOn", false);

        // On enlève le format d'activité pour l'inventaire
        playerInventory.inActivity = false;
    }

    private void InteractWithActivity()
    {
        if (IsOwner && inputManager.CanInteract && inTrigger)
        {
            // Start Current Activity Triggered
            inputManager.CanInteract = false;

            Debug.Log("interact");

            InteractActivity();
        }
    }

    /// <summary>
    /// Activité stand de tir, active et désactive le pistolet
    /// </summary>
    /// <param name="state"></param>
    public void Pistol(bool state)
    {
        if (IsOwner)
        {
            if (state)
            {
                if (PI.isCops) pistolCopPrefab.SetActive(true);
                else if (!PI.isCops) pistolRunnerPrefab.SetActive(true);
            }
            else if (!state)
            {
                if (PI.isCops) pistolCopPrefab.SetActive(false);
                else if (!PI.isCops) pistolRunnerPrefab.SetActive(false);
            }
        }
    }

    private void InteractActivity()
    {
        if (!playerInActivity)
        {
            // Si le joueur ne se trouve pas encore dans une activité
            playerInActivity = true;

            // Set up de l'inventaire pour le format d'activité
            playerInventory.inActivity = true;

            if (standTir) ShootingRange();

            if (piano) PianoManoir();
        }
        else if (playerInActivity) // remplacer par la touche échap pour le piano
        {
            if (standTir) ForceLeaveActivity();
        }
    }
    #endregion


    #region Activity
    private void ShootingRange()
    {
        Debug.Log("J'interragis avec le stand de tir");

        // On active le crosshair
        GM.panelShootingRange.SetActive(true);

        //Pistol(true);

        playerShoot.playerAnimator.SetBool("PistolOn", true);
    }

    private void PianoManoir()
    {
        Debug.Log("J'interragis avec le piano");

        GM.UIM.panelPiano.SetActive(true);
    }
    #endregion
}