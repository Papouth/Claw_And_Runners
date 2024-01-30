using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CapturePlayer : NetworkBehaviour
{
    [SerializeField] private GameObject zonz;
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
            NetworkObject playerPrefab = NetworkManager.Singleton.ConnectedClients[(ulong)idPlayerCaptured].PlayerObject;

            SetPlayerInJail((ulong)idPlayerCaptured);

            //TeleportInsideJailServerRpc((ulong)idPlayerCaptured, zonz.transform.position);
            //ClientTeleportClientRpc((ulong)idPlayerCaptured, zonz.transform.position);
        }
    }


    [ServerRpc]
    private void JailLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
    }
    
    private void SetPlayerInJail(ulong clientID)
    {
        NetworkObject playerPrefab = NetworkManager.Singleton.ConnectedClients[clientID].PlayerObject;
        playerPrefab.transform.position = zonz.transform.position;
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