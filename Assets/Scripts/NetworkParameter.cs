using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.VisualScripting;

public class NetworkParameter : MonoBehaviour
{
    #region Variables
    public int clientCount;
    public static List<GameObject> PlayersGameObjects = new List<GameObject>();
    public static List<string> PlayersNames = new List<string>();
    public List<string> NamesInfo = new List<string>();
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
        PlayersGameObjects.Add(playerGo);
        PlayersNames.Add(name);
        for (int i = 0; i < PlayersNames.Count; i++)
        {
            //Debug.Log("Noms des joueurs enregistré : " + PlayersNames[i]);

        }
    }

    public static void UnregisterPlayer(GameObject playerGo, string name)
    {
        PlayersGameObjects.Remove(playerGo);
        PlayersNames.Remove(name);
    }

    //TEST
    public void Update()
    {
        //NamesInfo = PlayersNames;

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