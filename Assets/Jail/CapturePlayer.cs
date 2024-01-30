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
        if (other.gameObject.CompareTag("runners"))
        {
            idPlayerCaptured = other.GetComponent<PlayerInfo>().playerId;

            other.gameObject.layer = 10;
            JailLayerServerRpc((ulong)idPlayerCaptured, 10);

            zonz = GameObject.Find("TheJail");
            Debug.Log("Zou direction la zonz");

            // ensuite transmettre au reste du server avec un call rpc
            if (IsOwner) TeleportInsideJailServerRpc((ulong)idPlayerCaptured, zonz.transform.position);
            ClientTeleportClientRpc((ulong)idPlayerCaptured, zonz.transform.position);
        }
    }


    [ServerRpc]
    private void JailLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
    }

    [ServerRpc]
    private void TeleportInsideJailServerRpc(ulong idPlayer, Vector3 pos)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.transform.position = pos;
        Debug.Log("Passe dans le serveur RPC de la jail");
    }

    [ClientRpc]
    private void ClientTeleportClientRpc(ulong idPlayer, Vector3 pos)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.transform.position = pos;
        Debug.Log("Client RPC Validé");
    }
}