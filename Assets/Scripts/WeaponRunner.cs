using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class WeaponRunner : NetworkBehaviour
{
    private InputManager inputManager;
    [SerializeField] private GameObject hitCollider;
    private bool justStarted;
    private PlayerInventory playerInventory;


    public override void OnNetworkSpawn()
    {
        inputManager = GetComponent<InputManager>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        // Vérifier aussi que le runner à en main son baton et qu'il ne se trouve pas dans une activité
        if (inputManager.CanSelect && IsOwner && !playerInventory.isSlot2 && !playerInventory.inActivity)
        {
            EnableColServerRpc();
        }
        else if (!inputManager.CanSelect && IsOwner && !justStarted)
        {
            justStarted = true;
            DisableColServerRpc();
        }
    }

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
}