using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponCop : NetworkBehaviour
{
    private InputManager inputManager;
    [SerializeField] private GameObject hitCollider;
    private VirtualJail VJ;
    private bool justStarted;


    private void Start()
    {
        //inputManager = GetComponent<InputManager>();
        VJ = GetComponent<VirtualJail>();
    }

    public override void OnNetworkSpawn()
    {
        inputManager = GetComponent<InputManager>();
        VJ = GetComponent<VirtualJail>();
    }


    private void Update()
    {
        // Vérifier aussi que le policier à en main son baton
        if (inputManager.CanSelect && IsOwner && VJ.prisonOn)
        {
            EnableColServerRpc();
            Debug.Log("Je te tape pour te mettre en prison");
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