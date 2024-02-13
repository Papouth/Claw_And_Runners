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

            JailLayerServerRpc((ulong)idPlayerCaptured, 10);

            zonz = GameObject.Find("TheJail");
            Debug.Log("Zou direction la zonz");

            // On téléporte le joueur en prison ICI

            //NetworkObject playerPrefab = NetworkManager.Singleton.ConnectedClients[(ulong)idPlayerCaptured].PlayerObject;
            //playerPrefab.transform.position = Position.Value;

            //SubmitPositionServerRpc((ulong)idPlayerCaptured);
        }
    }

    [ServerRpc]
    private void JailLayerServerRpc(ulong idPlayer, int layer)
    {
        //NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
        playerCaptured.layer = layer;

        JailLayerClientRpc(idPlayer, layer);
    }

    [ServerRpc]
    private void SubmitPositionServerRpc(ulong idPlayer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.transform.position = zonz.transform.position;
        Position.Value = zonz.transform.position;

        SubmitPositionClientRpc(idPlayer);
    }

    [ClientRpc]
    private void JailLayerClientRpc(ulong idPlayer, int layer)
    {
        //NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
        playerCaptured.layer = layer;
    }

    [ClientRpc]
    private void SubmitPositionClientRpc(ulong idPlayer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.transform.position = zonz.transform.position;
        Position.Value = zonz.transform.position;
    }
}