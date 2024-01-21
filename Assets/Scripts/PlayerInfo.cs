using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;


public class PlayerInfo : NetworkBehaviour
{
    #region Variables
    private TeamSelection TS;
    private LobbyManager LM;
    private bool equilibrageOn;
    private int prevCops;
    private int prevRunners;
    [Tooltip("true = chasseur | false = runner")] public bool isCops;
    public int isCopsInt;
    public string playerName;
    [HideInInspector] public int playerId;
    public bool tsReadySelection;
    public bool haveJail;
    private VirtualJail VJ;
    #endregion


    private void Start()
    {
        prevCops = 0;
        prevRunners = 0;
    }

    public override void OnNetworkSpawn()
    {
        TS = FindObjectOfType<TeamSelection>();

        LM = FindObjectOfType<LobbyManager>();

        VJ = GetComponent<VirtualJail>();
    }

    private void Update()
    {
        // Sert � connaitre l'id�ntit� de chaque joueur
        if (Input.GetMouseButtonDown(0) && IsOwner && TS.selectionStarted)
        {
            SendClientIDFunction();
        }


        if (IsOwner && !equilibrageOn)
        {
            equilibrageOn = true;

            TS.Equilibrage();

            if (LM == null) LM = FindObjectOfType<LobbyManager>();

            if (IsServer) TS.parcName.text = LM.joinedLobby.Name;
        }

        if (IsOwner)
        {
            if (TS.copsN.Value != prevCops)
            {
                TS.UpdateCopsNValue();

                TS.UpdateSelectionNames();

                prevCops = TS.copsN.Value;
            }

            if (TS.runnersN.Value != prevRunners)
            {
                TS.UpdateRunnersNValue();

                TS.UpdateSelectionNames();

                prevRunners = TS.runnersN.Value;
            }

            if (TS.requireNameUpdate)
            {
                TS.requireNameUpdate = false;

                TS.UpdateSelectionNames();
            }
        }

        if (TS.readySelection)
        {
            tsReadySelection = true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendClientIDServerRpc(ulong clientId)
    {
        // fonction a appel� pour d�clencher
        //SendClientIDFunction();
        Debug.Log("Client ayant cliqu� a l'ID : " + clientId);
        playerId = (int)clientId;
        NetworkParameter.lastIdSave = playerId;
    }

    public void SendClientIDFunction()
    {
        SendClientIDServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ClientRpc]
    public void UpdateServerPlayerNameClientRpc(string newplayername)
    {
        playerName = newplayername;
        gameObject.name = playerName;
    }

    [ClientRpc]
    public void UpdateServerInfoClientRpc(bool playerIsCops, int playerIsCopsInt)
    {
        isCops = playerIsCops;
        isCopsInt = playerIsCopsInt;

        if (isCops) gameObject.tag = "cops";
        else if (!isCops) gameObject.tag = "runners";
    }

    [ClientRpc]
    public void UpdateServerRoleJailClientRpc(bool playerJail)
    {
        haveJail = playerJail;

        if (!haveJail && VJ != null) Destroy(VJ);
        else if (haveJail && !VJ.enabled ) VJ.enabled = true;
    }
}