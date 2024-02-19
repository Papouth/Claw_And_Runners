using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ReleasePlayer : NetworkBehaviour
{
    private int idPlayerReleased; 


    private void OnTriggerEnter(Collider other)
    {
        // SI le joueur que je frappe � le layer captur�
        if (other.gameObject.layer == 10 && IsOwner)
        {
            idPlayerReleased = other.GetComponent<PlayerInfo>().playerId;

            // Layer 6 pour �tre un elfe libre
            other.gameObject.layer = 6;

            if (IsOwner) ReleaseLayerServerRpc((ulong)idPlayerReleased, 6);
        }
    }

    [ServerRpc]
    private void ReleaseLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;

        ReleaseLayerClientRpc(idPlayer, layer);
    }

    [ClientRpc]
    private void ReleaseLayerClientRpc(ulong idPlayer, int layer)
    {
        if (IsServer) NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
    }
}