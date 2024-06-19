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
    private PlayerInfo PI;
    private GameManager GM;
    private CharacterController CC;

    private bool ccState;

    public bool inTrigger;
    public bool playerInActivity;

    [Header("Activity Bool")]
    [HideInInspector] public bool piano;

    [HideInInspector] public bool porte;
    [HideInInspector] public bool doorIsOpen;
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
        GM = FindObjectOfType<GameManager>();

        CC = GetComponent<CharacterController>();
        ccState = true;

        inputManager = GetComponent<InputManager>();

        playerInventory = GetComponent<PlayerInventory>();

        PI = GetComponent<PlayerInfo>();
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
        if (!inTrigger && playerInActivity && piano)
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

        // On retire UI du piano
        GM.UIM.panelPiano.SetActive(false);

        // Bool player In Activity et standTir repassent en false
        playerInActivity = false;
        piano = false;

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

            if (porte) OpenDoor();
            else InteractActivity();
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

            if (piano) PianoManoir();
        }
    }
    #endregion


    #region Activity
    private void PianoManoir()
    {
        Debug.Log("J'interragis avec le piano");

        GM.UIM.panelPiano.SetActive(true);
    }

    private void OpenDoor()
    {
        // On récupère la bonne porte et on l'ouvre
        if (!doorIsOpen) doorIsOpen = true;
    }
    #endregion
}