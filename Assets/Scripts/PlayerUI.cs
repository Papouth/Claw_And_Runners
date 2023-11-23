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

        TS.InitTeamSelection();

        TS.Equilibrage();

        if (IsOwner)
        {
            UpdateTeamUIServerRpc(TS.copsLimit, TS.runnersLimit);
        }
    }

    [ServerRpc]
    private void UpdateTeamUIServerRpc(int copsLimit, int runnersLimit)
    {
        TS.copsLimit = copsLimit;
        TS.runnersLimit = runnersLimit;

        TS.UIMAJMaxPlayers();
    }

    //private void Update()
    //{
    //    if (IsOwner)
    //    {
    //        ShootServerRpc(new Vector3(transform.position.x, 1, transform.position.z + 1), Quaternion.identity);
    //    }
    //}
    //
    //[ServerRpc]
    //private void UpdateTeamUIServerRpc()
    //{
    //
    //    PlayShootAudio();
    //}
}