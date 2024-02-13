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
            JailLayerServerRpc((ulong)idPlayerCaptured, 10);

            zonz = GameObject.Find("TheJail");
            Debug.Log("Zou direction la zonz");

            // ensuite transmettre au reste du server avec un call rpc
            if (IsServer)
            {
                NetworkObject playerPrefab = NetworkManager.Singleton.ConnectedClients[(ulong)idPlayerCaptured].PlayerObject;

                SetPlayerInJail((ulong)idPlayerCaptured);

                SubmitPositionServerRpc((ulong)idPlayerCaptured);
                playerPrefab.transform.position = Position.Value;
            }

            //TeleportInsideJailServerRpc((ulong)idPlayerCaptured, zonz.transform.position);
            //ClientTeleportClientRpc((ulong)idPlayerCaptured, zonz.transform.position);
        }
    }

    [ServerRpc]
    private void SubmitPositionServerRpc(ulong idPlayer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.transform.position = zonz.transform.position;
        Position.Value = zonz.transform.position;
    }

    [ServerRpc]
    private void JailLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
    }

    private void SetPlayerInJail(ulong clientID)
    {
        if (IsServer)
        {
            NetworkObject playerPrefab = NetworkManager.Singleton.ConnectedClients[clientID].PlayerObject;
            playerPrefab.transform.position = zonz.transform.position;
        }
    }

    //[ServerRpc]
    //private void TeleportInsideJailServerRpc(ulong idPlayer, Vector3 pos)
    //{
    //    NetworkManager.ConnectedClients[idPlayer].PlayerObject.transform.position = pos;
    //    Debug.Log("Passe dans le serveur RPC de la jail");
    //}
    //
    //[ClientRpc]
    //private void ClientTeleportClientRpc(ulong idPlayer, Vector3 pos)
    //{
    //    NetworkManager.ConnectedClients[idPlayer].PlayerObject.transform.position = pos;
    //    Debug.Log("Client RPC Validé");
    //}
}