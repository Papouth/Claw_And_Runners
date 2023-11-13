using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class NetworkParameter : MonoBehaviour
{
    #region Variables
    public int clientCount;


    //// Team Selection Max Player Number
    //public NetworkVariable<int> copsLimit = new NetworkVariable<int>();
    //public NetworkVariable<int> runnersLimit = new NetworkVariable<int>();
    //
    //// Team Selection Actual Player Number
    //public NetworkVariable<int> copsN = new NetworkVariable<int>();
    //public NetworkVariable<int> runnersN = new NetworkVariable<int>();
    //
    //// Noms des joueurs de chaque équipe
    //[HideInInspector] public NetworkList<FixedString64Bytes> copsPlayerNameTxt = new NetworkList<FixedString64Bytes>();
    //[HideInInspector] public NetworkList<FixedString64Bytes> runnersPlayerNameTxt = new NetworkList<FixedString64Bytes>();
    #endregion

    private void Awake()
    {
        clientCount = 0;
    }

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



}