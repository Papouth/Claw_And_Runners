using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

public class CapturePlayer : NetworkBehaviour
{
    [SerializeField] private GameObject zonz;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    private int idPlayerCaptured;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("runners") && IsOwner)
        {
            idPlayerCaptured = other.GetComponent<PlayerInfo>().playerId;

            other.gameObject.layer = 10;

            if (IsOwner) JailLayerServerRpc((ulong)idPlayerCaptured, 10);

            zonz = GameObject.Find("TheJail");

            Debug.Log("Zou direction la zonz");
        }
    }

    [ServerRpc]
    private void JailLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;

        Debug.Log("SERVER CAPTURE PLAYER");

        JailLayerClientRpc(idPlayer, layer);
    }

    [ClientRpc]
    private void JailLayerClientRpc(ulong idPlayer, int layer)
    {
        if (IsServer) NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;

        Debug.Log("CLIENT CAPTURE PLAYER");
    }
}