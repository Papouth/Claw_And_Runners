using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerTeam : NetworkBehaviour
{
    private TeamSelection teamSelection;


    // Quand mon joueur apparait
    public override void OnNetworkSpawn()
    {
        teamSelection = FindObjectOfType<TeamSelection>();

        teamSelection.ShowHideUI();
    }
}