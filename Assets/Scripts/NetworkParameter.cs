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
    // Les noms des joueurs dans le lobby
    public static List<string> PlayersNames = new List<string>();

    // Les joueurs apr�s que l'on est r�cup�r� leurs nom et ID
    public static List<GameObject> PlayersIdentified = new List<GameObject>();
    // Les noms des joueurs class� et sauvegard�
    public static List<string> PlayersNamesIdentified = new List<string>();

    public static int counterClient = 0;

    public static int lastIdSave;
    public static string lastNameSavedInfo;
    public static GameObject playerGO;

    [HideInInspector] public static PlayerInfo[] myPlayersGo;

    #endregion

    private void Awake()
    {
        clientCount = 0;
    }

    #region Old
    public void StartHostButton()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClientButton()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StarServerButton()
    {
        NetworkManager.Singleton.StartServer();
    }
    #endregion

    //public static void RegisterPlayer(string name)
    //{
    //    PlayersNames.Add(name);
    //
    //    Debug.Log("Noms du nouveau joueurs enregistr� : " + PlayersNames[counterClient]);
    //
    //    counterClient++;
    //}

    //public static void UnregisterPlayer(string name)
    //{
    //    PlayersNames.Remove(name);
    //}

    /// <summary>
    /// Si le nom du joueur n'est pas encore sauvegard�, la fonction s'�x�cute
    /// </summary>
    /// <param name="playerName"></param>
    public static void SavePlayerInfo(string playerName)
    {
        lastNameSavedInfo = playerName;

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

                    break;
                }
            }
        }
    }

    public static void GetPlayerOnSelection()
    {
        myPlayersGo = FindObjectsOfType<PlayerInfo>();
        Debug.Log("Found " + myPlayersGo.Length + " with PlayerInfo script attached");
    }

    //TEST
    public void Update()
    {
        //if (PlayersGameObjects.Count != 0)
        //{
        //    // Pour chaque joueur pr�sent dans ma liste
        //    foreach (GameObject go in PlayersGameObjects)
        //    {
        //        for (int i = 0; i < PlayersGameObjects.Count; i++)
        //        {
        //            // Si l'ID de mon joueur est �gale � i, il s'agit donc du joueur i, et je lui attribue le composant playerinfo correspondant -> il l'a d�j�
        //            if (go.GetComponent<ClientNetworkTransform>().OwnerClientId == (ulong)i)
        //            {
        //
        //
        //                break;
        //            }
        //        }
        //
        //
        //    }
        //}
    }
}