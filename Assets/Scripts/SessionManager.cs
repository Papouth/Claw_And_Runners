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
    private FixedString32Bytes copWithJail;


    public override void OnNetworkSpawn()
    {
        TS = gameObject.GetComponent<TeamSelection>();
    }

    public void AttributionTag()
    {
        Invoke("Tagging", 1f);
    }

    private void Tagging()
    {
        #region Owner
        /*if (IsOwner)
        {
            Debug.Log(TS.copsNamesList[0] + " nom du flic");

            // Attribution de la prison à l'un des flics si entre 6 et 8 joueurs
            if (NetworkManager.ConnectedClientsList.Count >= 6)
            {
                var randomInt = Random.Range(1, 2);
                Debug.Log("policier choisit : " + randomInt);

                copWithJail = TS.copsNamesList[randomInt];
            }
            else if (NetworkManager.ConnectedClientsList.Count < 6)
            {
                // Attribution de la prison au seul policier présent dans la partie
                copWithJail = TS.copsNamesList[0];
            }

            foreach (NetworkClient item in NetworkManager.ConnectedClientsList)
            {
                PI = item.PlayerObject.gameObject.GetComponent<PlayerInfo>();

                if (TS.copsNamesList.Contains(PI.playerName))
                {
                    //Debug.Log(item.PlayerObject.name + " est un policier");

                    PI.isCops = true;
                    PI.isCopsInt = 1;

                    item.PlayerObject.gameObject.tag = "cops";

                    PI.UpdateServerInfoClientRpc(true, 1);

                    item.PlayerObject.gameObject.GetComponent<WeaponCop>().enabled = true;

                    // S'il s'agit du policier ayant la prison
                    if (PI.playerName == copWithJail)
                    {
                        // On enable le script de la prison sur le joueur
                        item.PlayerObject.gameObject.GetComponent<VirtualJail>().enabled = true;

                        PI.UpdateServerRoleJailClientRpc(true);
                    }
                    else if (PI.playerName != copWithJail)
                    {
                        Destroy(item.PlayerObject.gameObject.GetComponent<VirtualJail>());

                        PI.UpdateServerRoleJailClientRpc(false);
                    }
                }
                else if (TS.runnersNamesList.Contains(PI.playerName))
                {
                    //Debug.Log(item.PlayerObject.name + " est un voleur");

                    PI.isCops = false;
                    PI.isCopsInt = 2;

                    item.PlayerObject.gameObject.tag = "runners";

                    PI.UpdateServerInfoClientRpc(false, 2);
                    Destroy(item.PlayerObject.gameObject.GetComponent<VirtualJail>());

                    Destroy(item.PlayerObject.gameObject.GetComponent<WeaponCop>());
                    Destroy(item.PlayerObject.gameObject.GetComponentInChildren<CapturePlayer>().gameObject);
                    PI.UpdateServerRoleJailClientRpc(false);
                    PI.UpdateServerRoleCaptureClientRpc(false);
                }
            }
        }*/
        #endregion


        ////


        #region Server
        /*if (IsOwner)
        {
            Debug.Log(TS.copsNamesList[0] + " nom du flic");

            // Attribution de la prison à l'un des flics si entre 6 et 8 joueurs
            if (NetworkManager.ConnectedClientsList.Count >= 6)
            {
                var randomInt = Random.Range(1, 2);
                Debug.Log("policier choisit : " + randomInt);

                copWithJail = TS.copsNamesList[randomInt];
            }
            else if (NetworkManager.ConnectedClientsList.Count < 6)
            {
                // Attribution de la prison au seul policier présent dans la partie
                copWithJail = TS.copsNamesList[0];
            }

            foreach (NetworkClient item in NetworkManager.ConnectedClientsList)
            {
                PI = item.PlayerObject.gameObject.GetComponent<PlayerInfo>();

                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { item.ClientId }
                    }
                };

                if (TS.copsNamesList.Contains(PI.playerName))
                {
                    //Debug.Log(item.PlayerObject.name + " est un policier");

                    PI.isCops = true;
                    PI.isCopsInt = 1;

                    item.PlayerObject.gameObject.tag = "cops";

                    PI.UpdateServerInfoClientRpc(true, 1, clientRpcParams);

                    item.PlayerObject.gameObject.GetComponent<WeaponCop>().enabled = true;

                    // S'il s'agit du policier ayant la prison
                    if (PI.playerName == copWithJail)
                    {
                        // On enable le script de la prison sur le joueur
                        item.PlayerObject.gameObject.GetComponent<VirtualJail>().enabled = true;

                        PI.UpdateServerRoleJailClientRpc(true, clientRpcParams);
                    }
                    else if (PI.playerName != copWithJail)
                    {
                        Destroy(item.PlayerObject.gameObject.GetComponent<VirtualJail>());

                        PI.UpdateServerRoleJailClientRpc(false, clientRpcParams);
                    }
                }
                else if (TS.runnersNamesList.Contains(PI.playerName))
                {
                    //Debug.Log(item.PlayerObject.name + " est un voleur");

                    PI.isCops = false;
                    PI.isCopsInt = 2;

                    item.PlayerObject.gameObject.tag = "runners";

                    PI.UpdateServerInfoClientRpc(false, 2, clientRpcParams);
                    Destroy(item.PlayerObject.gameObject.GetComponent<VirtualJail>());

                    Destroy(item.PlayerObject.gameObject.GetComponent<WeaponCop>());
                    Destroy(item.PlayerObject.gameObject.GetComponentInChildren<CapturePlayer>().gameObject);
                    PI.UpdateServerRoleJailClientRpc(false, clientRpcParams);
                    PI.UpdateServerRoleCaptureClientRpc(false, clientRpcParams);
                }
            }
        }*/
        #endregion
    }
}