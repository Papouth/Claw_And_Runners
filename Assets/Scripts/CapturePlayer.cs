using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

public class CapturePlayer : NetworkBehaviour
{
    #region Variables
    [SerializeField] private GameObject zonz;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    private int idPlayerCaptured;
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
        if (other.gameObject.CompareTag("runners") && IsOwner)
        {
            audioSync.PlaySound(6);

            idPlayerCaptured = other.GetComponent<PlayerInfo>().playerId;

            other.gameObject.layer = 10;

            // RPC Call GM
            GMActionServerRpc();

            if (IsOwner) JailLayerServerRpc((ulong)idPlayerCaptured, 10);

            zonz = GameObject.FindWithTag("JailObject");

            //Debug.Log("Zou direction la zonz");
        }
    }
    #endregion

    #region Server RPC
    [ServerRpc]
    private void JailLayerServerRpc(ulong idPlayer, int layer)
    {
        NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;

        //Debug.Log("SERVER CAPTURE PLAYER");

        JailLayerClientRpc(idPlayer, layer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void GMActionServerRpc()
    {
        GM.actualRunnersCaptured++;
        GM.totalRunnersCaptured++;

        if (GM.actualRunnersCaptured == GM.runnersLimitGM.Value) GM.CheckCopsWin();
    }
    #endregion

    #region Client RPC
    [ClientRpc]
    private void JailLayerClientRpc(ulong idPlayer, int layer)
    {
        if (IsServer) NetworkManager.ConnectedClients[idPlayer].PlayerObject.gameObject.layer = layer;

        //Debug.Log("CLIENT CAPTURE PLAYER");
    }
    #endregion
}