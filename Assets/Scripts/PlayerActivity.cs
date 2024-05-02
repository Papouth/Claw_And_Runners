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


    public bool inTrigger;
    public bool playerInActivity;

    [Header("Activity Bool")]
    [HideInInspector] public bool standTir;


    [Header("Stand de Tir")]
    [SerializeField] private GameObject pistolCopPrefab;
    [SerializeField] private GameObject pistolRunnerPrefab;
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
        GM = FindObjectOfType<GameManager>();

        inputManager = GetComponent<InputManager>();

        playerInventory = GetComponent<PlayerInventory>();

        playerShoot = GetComponentInChildren<PlayerShoot>();

        PI = GetComponent<PlayerInfo>();

        pistolCopPrefab.SetActive(false);
        pistolRunnerPrefab.SetActive(false);
    }

    private void Update()
    {
        InteractWithActivity();

        LeaveActivity();
    }
    #endregion


    #region Customs Methods
    private void LeaveActivity()
    {
        if (!inTrigger && playerInActivity && standTir)
        {
            Debug.Log("Je quitte le stand de tir");

            // On d�sactive le crosshair
            GM.panelShootingRange.SetActive(false);

            // Bool player In Activity et standTir repassent en false
            playerInActivity = false;
            standTir = false;

            // On range le pistolet
            Pistol(false);

            // Fin animation pistolet
            playerShoot.playerAnimator.SetBool("PistolOn", false);

            // On enl�ve le format d'activit� pour l'inventaire
            playerInventory.inActivity = false;
        }
    }

    private void ForceLeaveActivity()
    {
        Debug.Log("Je quitte le stand de tir");

        // On d�sactive le crosshair
        GM.panelShootingRange.SetActive(false);

        // Bool player In Activity et standTir repassent en false
        playerInActivity = false;
        standTir = false;

        // On range le pistolet
        Pistol(false);

        // Fin animation pistolet
        playerShoot.playerAnimator.SetBool("PistolOn", false);

        // On enl�ve le format d'activit� pour l'inventaire
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
    /// Activit� stand de tir, active et d�sactive le pistolet
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
            // Si le joueur ne se trouve pas encore dans une activit�
            playerInActivity = true;

            // Set up de l'inventaire pour le format d'activit�
            playerInventory.inActivity = true;

            if (standTir) ShootingRange();
        }
        else if (playerInActivity)
        {
            ForceLeaveActivity();
        }
    }
    #endregion


    #region Activity
    private void ShootingRange()
    {
        Debug.Log("J'interragis avec le stand de tir");

        // On active le crosshair
        GM.panelShootingRange.SetActive(true);

        Pistol(true);

        playerShoot.playerAnimator.SetBool("PistolOn", true);
    }
    #endregion
}