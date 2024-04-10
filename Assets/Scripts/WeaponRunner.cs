using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class WeaponRunner : NetworkBehaviour
{
    #region Variables
    private InputManager inputManager;
    [SerializeField] private GameObject hitCollider;
    private bool justStarted;
    private PlayerInventory playerInventory;
    private AudioSync audioSync;
    private GameManager GM;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        audioSync = GetComponent<AudioSync>();
    }

    public override void OnNetworkSpawn()
    {
        inputManager = GetComponent<InputManager>();
        playerInventory = GetComponent<PlayerInventory>();
        GM = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        // Vérifier aussi que le runner à en main son baton et qu'il ne se trouve pas dans une activité
        if (inputManager.CanSelect && IsOwner && !playerInventory.isSlot2 && !playerInventory.inActivity)
        {
            inputManager.CanSelect = false;
            EnableColServerRpc();

            // SON
            if (!GM.cheat) audioSync.PlaySound(Random.Range(0, 5));
        }
        else if (!inputManager.CanSelect && IsOwner && !justStarted)
        {
            justStarted = true;
            DisableColServerRpc();
        }
    }
    #endregion


    #region Customs Methods
    [ServerRpc]
    private void EnableColServerRpc()
    {
        hitCollider.SetActive(true);
        Invoke("DisableColServerRpc", 0.5f);

        EnableColClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisableColServerRpc()
    {
        hitCollider.SetActive(false);
        inputManager.CanSelect = false;
    }

    [ClientRpc]
    private void EnableColClientRpc()
    {
        hitCollider.SetActive(true);
        Invoke("DisableColClientRpc", 0.5f);
    }

    [ClientRpc]
    private void DisableColClientRpc()
    {
        hitCollider.SetActive(false);
        inputManager.CanSelect = false;
    }
    #endregion
}