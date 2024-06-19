using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInventory : NetworkBehaviour
{
    #region Variables
    //public bool isSlot2;
    //public bool isSlot2Used;
    public bool skinChoosed;
    public bool animatorsReady;
    public bool inActivity; // vérifie si le joueur se trouve dans une activité

    private PlayerInfo PI;
    private InputManager inputManager;
    public Animator serverAnimator;
    public Animator clientAnimator;
    private Animator[] animatorListC;
    private Animator[] animatorListD;
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
        inputManager = GetComponent<InputManager>();

        PI = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (!animatorsReady && skinChoosed && IsOwner)
        {
            animatorsReady = true;

            if (PI.isCops)
            {
                serverAnimator = PI.playerCopPrefab.GetComponent<Animator>();

                animatorListC = PI.playerCopPrefab.GetComponentsInChildren<Animator>();
                clientAnimator = animatorListC[animatorListC.Length-1];

                AnimatorCopsServerRpc();
            }
            else if (!PI.isCops)
            {
                serverAnimator = PI.playerRunnerPrefab.GetComponent<Animator>();

                animatorListD = PI.playerRunnerPrefab.GetComponentsInChildren<Animator>();
                clientAnimator = animatorListD[animatorListD.Length-1];

                AnimatorRunnersServerRpc();
            }

            // Slot 1 par défaut
            //isSlot2 = false;
            //serverAnimator.SetBool(blabla)
            //clientAnimator.SetBool(blabla)
        }

        //if (skinChoosed && animatorsReady) ChangeSlot();
    }
    #endregion


    #region Customs Methods
    //public void ChangeSlot()
    //{
    //    if (inputManager.CanSlot1 && isSlot2 && IsOwner)
    //    {
    //        //Debug.Log("Baton");
    //
    //        // Switch pour le Slot 1
    //        inputManager.CanSlot1 = false;
    //        isSlot2 = false;
    //
    //
    //        // On prend l'objet du slot 1
    //        //serverAnimator.SetBool(blabla)
    //        //clientAnimator.SetBool(blabla)
    //
    //
    //        ChangeSlotServerRpc(isSlot2);
    //    }
    //    else if (inputManager.CanSlot2 && !isSlot2 && !isSlot2Used && IsOwner)
    //    {
    //        //Debug.Log("Pouvoir");
    //
    //        // Switch pour le Slot 2
    //        inputManager.CanSlot2 = false;
    //        isSlot2 = true;
    //
    //
    //        // On prend l'objet du slot 2
    //        //serverAnimator.SetBool(blabla)
    //        //clientAnimator.SetBool(blabla)
    //
    //
    //        ChangeSlotServerRpc(isSlot2);
    //    }
    //
    //    if (inputManager.CanSlot1) inputManager.CanSlot1 = false;
    //    else if (inputManager.CanSlot2) inputManager.CanSlot2 = false;
    //
    //    if (inputManager.ScrollMouse.y != 0 && IsOwner)
    //    {
    //        // Switch sur l'autre Slot
    //        if (isSlot2)
    //        {
    //            //Debug.Log("Baton molette");
    //
    //            // Switch pour le slot 1
    //            isSlot2 = false;
    //
    //
    //            // On prend l'objet du slot 1
    //            //serverAnimator.SetBool(blabla)
    //            //clientAnimator.SetBool(blabla)
    //
    //
    //            ChangeSlotServerRpc(isSlot2);
    //        }
    //        else if (!isSlot2 && !isSlot2Used)
    //        {
    //            //Debug.Log("Pouvoir molette");
    //
    //            // Switch pour le slot 2
    //            isSlot2 = true;
    //
    //
    //            // On prend l'objet du slot 2
    //            //serverAnimator.SetBool(blabla)
    //            //clientAnimator.SetBool(blabla)
    //
    //
    //            ChangeSlotServerRpc(isSlot2);
    //        }
    //    }
    //}
    #endregion

    #region ServerRpc
    [ServerRpc]
    private void AnimatorCopsServerRpc()
    {
        serverAnimator = PI.playerCopPrefab.GetComponent<Animator>();

        animatorListC = PI.playerCopPrefab.GetComponentsInChildren<Animator>();
        clientAnimator = animatorListC[animatorListC.Length-1];

        AnimatorCopsClientRpc();
    }

    [ServerRpc]
    private void AnimatorRunnersServerRpc()
    {
        serverAnimator = PI.playerRunnerPrefab.GetComponent<Animator>();

        animatorListD = PI.playerRunnerPrefab.GetComponentsInChildren<Animator>();
        clientAnimator = animatorListD[animatorListD.Length-1];

        AnimatorRunnersClientRpc();
    }


    //[ServerRpc(RequireOwnership = false)]
    //private void ChangeSlotServerRpc(bool slot)
    //{
    //    if (slot)
    //    {
    //        // Switch pour le slot 2
    //
    //        // On prend l'objet du slot 2
    //        //serverAnimator.SetBool(blabla)
    //        //clientAnimator.SetBool(blabla)
    //    }
    //    else if (!slot)
    //    {
    //        // Switch pour le slot 1
    //
    //        // On prend l'objet du slot 1
    //        //serverAnimator.SetBool(blabla)
    //        //clientAnimator.SetBool(blabla)
    //    }
    //
    //    ChangeSlotClientRpc(slot);
    //}
    #endregion

    #region ClientRpc
    //[ClientRpc]
    //private void ChangeSlotClientRpc(bool stateSlot)
    //{
    //    if (stateSlot)
    //    {
    //        //Debug.Log("Pouvoir ClientRpc");
    //
    //        // Switch pour le slot 2
    //
    //        // On prend l'objet du slot 2
    //        //serverAnimator.SetBool(blabla)
    //        //clientAnimator.SetBool(blabla)
    //    }
    //    else if (!stateSlot)
    //    {
    //        //Debug.Log("Baton ClientRpc");
    //
    //        // Switch pour le slot 1
    //
    //        // On prend l'objet du slot 1
    //        //serverAnimator.SetBool(blabla)
    //        //clientAnimator.SetBool(blabla)
    //    }
    //}

    [ClientRpc]
    private void AnimatorCopsClientRpc()
    {
        serverAnimator = PI.playerCopPrefab.GetComponent<Animator>();

        animatorListC = PI.playerCopPrefab.GetComponentsInChildren<Animator>();
        clientAnimator = animatorListC[animatorListC.Length-1];
    }

    [ClientRpc]
    private void AnimatorRunnersClientRpc()
    {
        serverAnimator = PI.playerRunnerPrefab.GetComponent<Animator>();

        animatorListD = PI.playerRunnerPrefab.GetComponentsInChildren<Animator>();
        clientAnimator = animatorListD[animatorListD.Length-1];
    }
    #endregion
}