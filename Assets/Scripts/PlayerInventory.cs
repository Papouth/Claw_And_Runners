using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInventory : NetworkBehaviour
{
    #region Variables
    public bool skinChoosed;
    public bool animatorsReady;
    public bool inActivity; // vérifie si le joueur se trouve dans une activité

    private PlayerInfo PI;
    public Animator serverAnimator;
    public Animator clientAnimator;
    private Animator[] animatorListC;
    private Animator[] animatorListD;
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
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
        }
    }
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
    #endregion

    #region ClientRpc
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