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
    private GameObject playerCaptured;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("runners") && IsOwner)
        {
            idPlayerCaptured = other.GetComponent<PlayerInfo>().playerId;

            other.gameObject.layer = 10;
            playerCaptured = other.gameObject;


            if (IsOwner) JailLayerServerRpc((ulong)idPlayerCaptured, 10);

            zonz = GameObject.Find("TheJail");

            Debug.Log("Zou direction la zonz");
        }
    }

    [ServerRpc]
    private void JailLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
    }

    [ClientRpc]
    private void JailLayerClientRpc(ulong idPlayer, int layer)
    {
        playerCaptured.layer = layer;
    }
}