using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Netcode;
using Unity.Collections;

public class SessionManager : NetworkBehaviour
{
    private PlayerInfo PI;
    private TeamSelection TS;


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

                Debug.Log(PI.playerName + " player en cours de traitement");

                if (TS.copsPlayerNameTxt.Contains(PI.playerName))
                {
                    Debug.Log("S1");
                    PI.isCops = true;
                }
                else if (TS.runnersPlayerNameTxt.Contains(PI.playerName))
                {
                    Debug.Log("S2");
                    PI.isCops = false;
                }

                Debug.Log(item.PlayerObject.name + " est : " + PI.isCops);
            }
        }
    }
}