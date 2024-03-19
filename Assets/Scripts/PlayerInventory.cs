using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInventory : NetworkBehaviour
{
    #region Variables
    [HideInInspector] public bool isSlot2;
    [HideInInspector] public bool isSlot2Used;
    [HideInInspector] public bool skinChoosed;
    private bool animatorsReady;

    private PlayerInfo PI;
    private InputManager inputManager;
    private Animator serverAnimator;
    private Animator clientAnimator;
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
        PI = GetComponent<PlayerInfo>();

        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (!animatorsReady && skinChoosed)
        {
            animatorsReady = true;

            if (PI.isCops)
            {
                serverAnimator = PI.playerCopPrefab.GetComponent<Animator>();

                clientAnimator = PI.playerCopPrefab.GetComponentInChildren<Animator>();
            }
            else if (!PI.isCops)
            {
                serverAnimator = PI.playerRunnerPrefab.GetComponent<Animator>();
                                          
                clientAnimator = PI.playerRunnerPrefab.GetComponentInChildren<Animator>();
            }
        }

        if (skinChoosed && animatorsReady) ChangeSlot();
    }
    #endregion

    #region Customs Methods
    public void ChangeSlot()
    {
        if (inputManager.TakeSlot1 && isSlot2)
        {
            Debug.Log("Baton");

            // Switch pour le Slot 1
            inputManager.TakeSlot1 = false;
            isSlot2 = false;


            // On prend l'objet du slot 1
            //serverAnimator.SetBool(blabla)
            //clientAnimator.SetBool(blabla)


            ChangeSlotServerRpc(isSlot2);
        }
        else if (inputManager.TakeSlot2 && !isSlot2 && !isSlot2Used)
        {
            Debug.Log("Pouvoir");

            // Switch pour le Slot 2
            inputManager.TakeSlot2 = false;
            isSlot2 = true;


            // On prend l'objet du slot 2
            //serverAnimator.SetBool(blabla)
            //clientAnimator.SetBool(blabla)


            ChangeSlotServerRpc(isSlot2);
        }

        if (inputManager.ScrollMouse.y != 0)
        {
            // Switch sur l'autre Slot
            if (isSlot2)
            {
                Debug.Log("Baton");

                // Switch pour le slot 1
                isSlot2 = false;


                // On prend l'objet du slot 1
                //serverAnimator.SetBool(blabla)
                //clientAnimator.SetBool(blabla)


                ChangeSlotServerRpc(isSlot2);
            }
            else if (!isSlot2 && !isSlot2Used)
            {
                Debug.Log("Pouvoir");

                // Switch pour le slot 2
                isSlot2 = true;


                // On prend l'objet du slot 2
                //serverAnimator.SetBool(blabla)
                //clientAnimator.SetBool(blabla)


                ChangeSlotServerRpc(isSlot2);
            }
        }
    }
    #endregion

    #region ServerRpc
    [ServerRpc(RequireOwnership = false)]
    private void ChangeSlotServerRpc(bool slot)
    {
        if (slot)
        {
            // Switch pour le slot 2
            isSlot2 = true;


            // On prend l'objet du slot 2
            //serverAnimator.SetBool(blabla)
            //clientAnimator.SetBool(blabla)
        }
        else if (!slot)
        {
            // Switch pour le slot 1
            isSlot2 = false;


            // On prend l'objet du slot 1
            //serverAnimator.SetBool(blabla)
            //clientAnimator.SetBool(blabla)
        }

        ChangeSlotClientRpc(slot);
    }
    #endregion

    #region ClientRpc
    [ClientRpc]
    private void ChangeSlotClientRpc(bool slot)
    {
        if (slot)
        {
            Debug.Log("Pouvoir");

            // Switch pour le slot 2
            isSlot2 = true;


            // On prend l'objet du slot 2
            //serverAnimator.SetBool(blabla)
            //clientAnimator.SetBool(blabla)
        }
        else if (!slot)
        {
            Debug.Log("Baton");

            // Switch pour le slot 1
            isSlot2 = false;


            // On prend l'objet du slot 1
            //serverAnimator.SetBool(blabla)
            //clientAnimator.SetBool(blabla)
        }
    }
    #endregion
}