using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkParameter : MonoBehaviour
{
    public int clientCount;

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