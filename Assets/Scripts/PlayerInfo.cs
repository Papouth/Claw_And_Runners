using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.Collections;



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
    private WeaponCop WC;
    public bool playerCop;
    public GameObject captureCol;

    private FixedString32Bytes copWithJail;
    private bool status;
    private bool setPlayerInJail;
    private GameObject zonz;
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

        WC = GetComponent<WeaponCop>();

        captureCol = GetComponentInChildren<CapturePlayer>().gameObject;
    }

    private void Update()
    {
        // Sert à connaitre l'idéntité de chaque joueur
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

            SetPlayerInJail();
        }

        if (TS.readySelection)
        {
            tsReadySelection = true;
        }

        if (TS.tagSetup && !status)
        {
            status = true;
            Invoke("StatusPlayer", 1f);
        }
    }

    private void SetPlayerInJail()
    {
        if (gameObject.layer == 10 && !setPlayerInJail)
        {
            Debug.Log("PLayer info pour aller en prison");

            if (zonz == null)
            {
                zonz = GameObject.Find("TheJail");
            }

            setPlayerInJail = true;

            JailLayerServerRpc(10);

            gameObject.transform.position = zonz.transform.position;

            SubmitPositionServerRpc(zonz.transform.position);
        }
    }

    /// <summary>
    /// Gère les tags et les scripts du joueur
    /// </summary>
    private void StatusPlayer()
    {
        if (IsOwner)
        {
            if (TS.copsNamesList.Contains(playerName))
            {
                if (TS.copsNamesList[0] == playerName)
                {
                    // Attribution de la prison au premier policier présent dans la partie
                    copWithJail = TS.copsNamesList[0];
                }

                isCops = true;
                isCopsInt = 1;

                gameObject.tag = "cops";

                InfoServerRpc(true, 1);

                WC.enabled = true;
                captureCol.SetActive(false);

                // S'il s'agit du policier ayant la prison
                if (playerName == copWithJail)
                {
                    // On enable le script de la prison sur le joueur
                    VJ.enabled = true;

                    RoleJailServerRpc(true);
                }
                else if (playerName != copWithJail)
                {
                    Destroy(VJ);

                    RoleJailServerRpc(false);
                }
            }
            else if (TS.runnersNamesList.Contains(playerName))
            {
                isCops = false;
                isCopsInt = 2;

                gameObject.tag = "runners";

                InfoServerRpc(false, 2);

                Destroy(VJ);

                Destroy(WC);
                Destroy(captureCol);

                RoleJailServerRpc(false);
                RoleCaptureServerRpc(false);
            }
        }
    }

    #region ServerRpc
    [ServerRpc]
    private void JailLayerServerRpc(int layer)
    {
        gameObject.layer = layer;

        JailLayerClientRpc(layer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitPositionServerRpc(Vector3 pos)
    {
        gameObject.transform.position = pos;

        SubmitPositionClientRpc(zonz.transform.position);
    }

    [ServerRpc]
    public void InfoServerRpc(bool playerIsCops, int playerIsCopsInt)
    {
        isCopsInt = playerIsCopsInt;
        isCops = playerIsCops;

        Debug.Log("Passe TEST serverInfoClientRpc");

        if (playerIsCops)
        {
            gameObject.tag = "cops";
            Debug.Log("playerInfo TEST tag Cops");
        }
        else if (!playerIsCops)
        {
            gameObject.tag = "runners";
            Debug.Log("playerInfo TEST tag Runners");
        }

        UpdateServerInfoClientRpc(isCops, isCopsInt);
    }

    [ServerRpc]
    public void RoleJailServerRpc(bool playerJail)
    {
        haveJail = playerJail;

        if (!haveJail && VJ != null) Destroy(VJ);
        else if (haveJail && !VJ.enabled) VJ.enabled = true;

        UpdateServerRoleJailClientRpc(haveJail);
    }

    [ServerRpc]
    public void RoleCaptureServerRpc(bool playerCoporNot)
    {
        playerCop = playerCoporNot;

        if (!playerCop && WC != null) Destroy(WC);
        else if (playerCop && !WC.enabled) WC.enabled = true;

        if (!playerCop && captureCol != null) Destroy(captureCol);

        UpdateServerRoleCaptureClientRpc(playerCop);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendClientIDServerRpc(ulong clientId)
    {
        // fonction a appelé pour déclencher
        Debug.Log("Client ayant cliqué a l'ID : " + clientId);
        playerId = (int)clientId;
        NetworkParameter.lastIdSave = playerId;
    }
    #endregion

    public void SendClientIDFunction()
    {
        SendClientIDServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    #region ClientRpc
    [ClientRpc]
    private void JailLayerClientRpc(int layer)
    {
        gameObject.layer = layer;
    }

    [ClientRpc]
    private void SubmitPositionClientRpc(Vector3 pos)
    {
        gameObject.transform.position = pos;
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
        isCopsInt = playerIsCopsInt;
        isCops = playerIsCops;

        if (playerIsCops)
        {
            gameObject.tag = "cops";
            //Debug.Log(TS.copsNamesList[0] + "playerInfo tag Cops");
        }
        else if (!playerIsCops)
        {
            gameObject.tag = "runners";
            //Debug.Log(TS.copsNamesList[0] + "playerInfo tag Runners");
        }
    }

    [ClientRpc]
    public void UpdateServerRoleJailClientRpc(bool playerJail)
    {
        haveJail = playerJail;

        if (!haveJail && VJ != null) Destroy(VJ);
        else if (haveJail && !VJ.enabled) VJ.enabled = true;
    }

    [ClientRpc]
    public void UpdateServerRoleCaptureClientRpc(bool playerCoporNot)
    {
        playerCop = playerCoporNot;

        if (!playerCop && WC != null) Destroy(WC);
        else if (playerCop && !WC.enabled) WC.enabled = true;

        if (!playerCop && captureCol != null) Destroy(captureCol);
    }
    #endregion
}