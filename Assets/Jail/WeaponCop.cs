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
        inputManager = GetComponent<InputManager>();
        VJ = GetComponent<VirtualJail>();
    }

    private void Update()
    {
        // Vérifier aussi que le policier à en main son baton
        if (inputManager.CanSelect && IsOwner && VJ.prisonOn)
        {
            EnableCol();
            Debug.Log("Je te tape pour te mettre en prison");
        }
        else if (!inputManager.CanSelect && IsOwner && !justStarted)
        {
            justStarted = true;
            DisableCol();
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