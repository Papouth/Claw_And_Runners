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
    #endregion


    private void Start()
    {
        prevCops = 0;
        prevRunners = 0;
    }

    private void OnEnable()
    {
        LM = FindObjectOfType<LobbyManager>();

        //foreach (Player player in LM.joinedLobby.Players)
        //{
        //    NetworkParameter.RegisterPlayer(gameObject, player.Data["PlayerName"].Value.ToString());
        //
        //    Debug.Log("Un nouveau joueur viens d'entrer dans le parc et son nom est : " + player.Data["PlayerName"].Value.ToString());
        //
        //    playerName = player.Data["PlayerName"].Value.ToString();
        //
        //    gameObject.name = playerName;
        //}
    }

    //private void OnDisable()
    //{
    //    NetworkParameter.UnregisterPlayer(gameObject, LM.playerName);
    //}

    public override void OnNetworkSpawn()
    {
        TS = FindObjectOfType<TeamSelection>();
    }

    private void Update()
    {
        if (IsOwner && !equilibrageOn)
        {
            equilibrageOn = true;

            TS.Equilibrage();

            if (LM == null) LM = FindObjectOfType<LobbyManager>();
               
            TS.parcName.text = LM.joinedLobby.Name;
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

            if (TS.readySelection && !TS.nameIsSetup)
            {
                TS.nameIsSetup = true;
                playerName = LM.playerName;
                Debug.Log("Mon nom est : " + playerName);
            }
        }

        if (isCopsInt == 0) Debug.Log("Je suis rien zebi");
        if (isCopsInt == 1) Debug.Log("Je suis policier");
        if (isCopsInt == 2) Debug.Log("Je suis voleur");
    }
}