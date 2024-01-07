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
        
                Debug.Log(PI.playerName + " player en cours de traitement");
        
                if (TS.copsPlayerNameTxt.Contains(PI.playerName))
                {
                    Debug.Log("S1");
                    PI.isCops = true;
                    PI.isCopsInt = 1;
                }
                else if (TS.runnersPlayerNameTxt.Contains(PI.playerName))
                {
                    Debug.Log("S2");
                    PI.isCops = false;
                    PI.isCopsInt = 2;
                }
        
                Debug.Log(item.PlayerObject.name + " est : " + PI.isCops);
            }

            //PlayerInLobby();
        }
    }

    /// <summary>
    /// Ne fonctionne pas, ne passse même pas dedans
    /// </summary>
    private void PlayerInLobby()
    {
        if (LM.joinedLobby != null && LM.joinedLobby.Players != null)
        {
            Debug.Log(LM.joinedLobby.Players.Count + " est le nombre de joueurs dans le lobby");

            foreach (Player player in LM.joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // Le joueur se trouve dans le lobby
                    if (TS.copsPlayerNameTxt.Contains(player.Profile.Name))
                    {
                        Debug.Log(player.Profile.Name + " est : " + " un policier");
                    }
                    if (TS.runnersPlayerNameTxt.Contains(player.Profile.Name))
                    {
                        Debug.Log(player.Profile.Name + " est : " + " un voleur");
                    }
                }
            }
        }
    }
}