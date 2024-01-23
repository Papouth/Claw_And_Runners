using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponCop : NetworkBehaviour
{
    private InputManager inputManager;
    [SerializeField] private GameObject hitCollider;
    private VirtualJail VJ;


    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        VJ = GetComponent<VirtualJail>();
    }

    private void Update()
    {
        // V�rifier aussi que le policier � en main son baton
        if (inputManager.CanSelect && IsOwner && VJ.prisonOn)
        {
            EnableCol();
        }
    }

    private void EnableCol()
    {
        hitCollider.SetActive(true);
        Invoke("DisableCol", 1f);
    }

    private void DisableCol()
    {
        hitCollider.SetActive(false);
        inputManager.CanSelect = false;
    }
}