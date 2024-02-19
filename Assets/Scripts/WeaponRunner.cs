using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class WeaponRunner : NetworkBehaviour
{
    private InputManager inputManager;
    [SerializeField] private GameObject hitCollider;
    private bool justStarted;


    public override void OnNetworkSpawn()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        // V�rifier aussi que le runner � en main son baton
        if (inputManager.CanSelect && IsOwner)
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