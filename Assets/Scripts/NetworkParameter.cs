using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.VisualScripting;
using Unity.Services.Lobbies.Models;

/// <summary>
/// R�f�rence les joueurs en r�cup�rant leur nom et leur ID
/// </summary>
public class NetworkParameter : MonoBehaviour
{
    #region Variables
    public int clientCount;
    public int runnersCounter;
    // Les noms des joueurs dans le lobby
    public static List<string> PlayersNames = new List<string>();

    // Les joueurs apr�s que l'on est r�cup�r� leurs nom et ID
    public static List<GameObject> PlayersIdentified = new List<GameObject>();
    // Les noms des joueurs class� et sauvegard�
    public static List<string> PlayersNamesIdentified = new List<string>();

    public static int lastIdSave;
    public static GameObject playerGO;

    [HideInInspector] public static PlayerInfo[] myPlayersGo;
    #endregion


    /// <summary>
    /// Si le nom du joueur n'est pas encore sauvegard�, la fonction s'�x�cute
    /// </summary>
    /// <param name="playerName"></param>
    public static void SavePlayerInfo(string playerName)
    {
        if (!PlayersNamesIdentified.Contains(playerName))
        {
            foreach (var playerGo in myPlayersGo)
            {
                if (playerGo.gameObject.GetComponent<ClientNetworkTransform>().OwnerClientId == (ulong)lastIdSave)
                {
                    PlayersIdentified.Add(playerGo.gameObject);
                    PlayersNamesIdentified.Add(playerName);

                    playerGo.GetComponent<PlayerInfo>().playerName = playerName;
                    playerGo.gameObject.name = playerName;

                    // test semble fonctionnel
                    playerGo.GetComponent<PlayerInfo>().UpdateServerPlayerNameClientRpc(playerName);

                    break;
                }
            }
        }
    }

    public static void GetPlayerOnSelection()
    {
        myPlayersGo = FindObjectsOfType<PlayerInfo>();
    }
}