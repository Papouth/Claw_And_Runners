using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerUI : NetworkBehaviour
{
   private TeamSelection TS;
   
   private void Start()
   {
       TS = FindObjectOfType<TeamSelection>();
   
   
       //if (IsOwner)
       //{
       //     TS.Equilibrage();
       //}
   }

    private void Update()
    {
        if (IsOwner)
        {
            TS.Equilibrage();
        }
    }

    //ServerRpc]
    //rivate void UpdateTeamUIServerRpc(int copsLimit, int runnersLimit)
    //
    //   TS.copsLimit = copsLimit;
    //   TS.runnersLimit = runnersLimit;
    //
    //   TS.UIMAJMaxPlayers();

}