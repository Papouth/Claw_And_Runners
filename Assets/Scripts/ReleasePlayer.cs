using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ReleasePlayer : NetworkBehaviour
{
    #region Varaibles
    private int idPlayerReleased;
    private AudioSync audioSync;
    private GameManager GM;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        audioSync = GetComponentInParent<AudioSync>();
    }

    public override void OnNetworkSpawn()
    {
        GM = FindObjectOfType<GameManager>();
    }
    #endregion


    #region Customs Methods
    private void OnTriggerEnter(Collider other)
    {
        // SI le joueur que je frappe à le layer capturé
        if (other.gameObject.layer == 10 && IsOwner)
        {
            audioSync.PlaySound(7);

            idPlayerReleased = other.GetComponent<PlayerInfo>().playerId;

            // Layer 6 pour être un elfe libre
            other.gameObject.layer = 6;

            // RPC Call GM
            GMRPActionServerRpc();

            if (IsOwner) ReleaseLayerServerRpc((ulong)idPlayerReleased, 6);
        }
    }
    #endregion

    #region Server RPC
    [ServerRpc]
    private void ReleaseLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;

        ReleaseLayerClientRpc(idPlayer, layer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void GMRPActionServerRpc()
    {
        GM.actualRunnersCaptured--;
        GM.totalRunnersReleased++;
    }
    #endregion


    #region Client RPC
    [ClientRpc]
    private void ReleaseLayerClientRpc(ulong idPlayer, int layer)
    {
        if (IsServer) NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;
    }
    #endregion
}