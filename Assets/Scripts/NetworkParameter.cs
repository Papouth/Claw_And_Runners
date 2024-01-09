using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.VisualScripting;
using Unity.Services.Lobbies.Models;

public class NetworkParameter : MonoBehaviour
{
    #region Variables
    public int clientCount;
    public static List<GameObject> PlayersGameObjects = new List<GameObject>();
    public static List<string> PlayersNames = new List<string>();
    public static int counterClient = 0;
    public static int lastIdSave;
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

    public static void RegisterPlayer(GameObject playerGo, string name)
    {
        //PlayersGameObjects.Add(playerGo);
        //
        //Debug.Log("STEP A : " + playerGo);

        PlayersNames.Add(name);

        //Debug.Log("STEP B : " + name);

        Debug.Log("Noms du nouveau joueurs enregistré : " + PlayersNames[counterClient]);


        //PlayersGameObjects[counterClient].GetComponent<PlayerInfo>().playerName = PlayersNames[counterClient];
        //
        //Debug.Log("STEP 1 : " + PlayersGameObjects[counterClient].GetComponent<PlayerInfo>().playerName);
        //
        //PlayersGameObjects[counterClient].name = PlayersNames[counterClient];
        //
        //Debug.Log("STEP 2 : " + PlayersGameObjects[counterClient].name);
        //
        //counterClient++;
        //Debug.Log(counterClient + " voici le compteur de clients");

        counterClient++;
    }

    public static void UnregisterPlayer(GameObject playerGo, string name)
    {
        PlayersGameObjects.Remove(playerGo);
        PlayersNames.Remove(name);
    }

    //TEST
    public void Update()
    {
        //if (PlayersGameObjects.Count != 0)
        //{
        //    // Pour chaque joueur présent dans ma liste
        //    foreach (GameObject go in PlayersGameObjects)
        //    {
        //        for (int i = 0; i < PlayersGameObjects.Count; i++)
        //        {
        //            // Si l'ID de mon joueur est égale à i, il s'agit donc du joueur i, et je lui attribue le composant playerinfo correspondant -> il l'a déjà
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