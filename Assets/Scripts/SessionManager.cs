using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Netcode;
using Unity.Collections;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;

public class SessionManager : NetworkBehaviour
{
    private PlayerInfo PI;
    private TeamSelection TS;
    [SerializeField] private LobbyManager LM;


    public override void OnNetworkSpawn()
    {
        TS = gameObject.GetComponent<TeamSelection>();
    }

    public void AttributionTag()
    {
        if (IsOwner)
        {
            foreach (NetworkClient item in NetworkManager.ConnectedClientsList)
            {
                PI = item.PlayerObject.gameObject.GetComponent<PlayerInfo>();

                //Debug.Log(PI.playerName + " player en cours de traitement");

                if (TS.copsNamesList.Contains(PI.playerName))
                {
                    Debug.Log(item.PlayerObject.name + " est un policier");

                    PI.isCops = true;
                    PI.isCopsInt = 1;
                }
                else if (TS.runnersNamesList.Contains(PI.playerName))
                {
                    Debug.Log(item.PlayerObject.name + " est un voleur" );

                    PI.isCops = false;
                    PI.isCopsInt = 2;
                }
            }
        }
    }
}