using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponCop : NetworkBehaviour
{
    #region Variables
    private InputManager inputManager;
    [SerializeField] private GameObject hitCollider;
    private VirtualJail VJ;
    private bool justStarted;
    private PlayerInventory playerInventory;
    private AudioSync audioSync;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        VJ = GetComponent<VirtualJail>();
        audioSync = GetComponent<AudioSync>();
    }

    public override void OnNetworkSpawn()
    {
        inputManager = GetComponent<InputManager>();
        VJ = GetComponent<VirtualJail>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        // Vérifier aussi que le policier à en main son baton et qu'il ne se trouve pas dans une activité
        if (inputManager.CanSelect && IsOwner && VJ.prisonOn && !playerInventory.inActivity)
        {
            inputManager.CanSelect = false;

            // ANIM
            if (playerInventory.animatorsReady)
            {
                playerInventory.serverAnimator.SetTrigger("Attack");
                playerInventory.clientAnimator.SetTrigger("Attack");

                UpdateAnimServerRpc();
            }

            EnableColServerRpc();

            // SON
            audioSync.PlaySound(Random.Range(0, 5));
        }
        else if (!inputManager.CanSelect && IsOwner && !justStarted)
        {
            justStarted = true;
            DisableColServerRpc();

            // ANIM
            if (playerInventory.animatorsReady)
            {
                playerInventory.serverAnimator.ResetTrigger("Attack");
                playerInventory.clientAnimator.ResetTrigger("Attack");
            }
        }
    }
    #endregion


    #region ServerRpc
    [ServerRpc]
    private void EnableColServerRpc()
    {
        hitCollider.SetActive(true);
        Invoke("DisableColServerRpc", 0.03f);

        EnableColClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisableColServerRpc()
    {
        hitCollider.SetActive(false);
        inputManager.CanSelect = false;
    }

    [ServerRpc]
    public void UpdateAnimServerRpc()
    {
        playerInventory.serverAnimator.SetTrigger("Attack");
        playerInventory.clientAnimator.SetTrigger("Attack");

        UpdateAnimClientRpc();
    }
    #endregion


    #region ClientRpc
    [ClientRpc]
    private void EnableColClientRpc()
    {
        hitCollider.SetActive(true);
        Invoke("DisableColClientRpc", 0.03f);
    }

    [ClientRpc]
    private void DisableColClientRpc()
    {
        hitCollider.SetActive(false);
        inputManager.CanSelect = false;
    }

    [ClientRpc]
    private void UpdateAnimClientRpc()
    {
        playerInventory.clientAnimator.SetTrigger("Attack");
    }
    #endregion
}