using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerInfo : NetworkBehaviour
{
    #region Variables
    private TeamSelection TS;
    private LobbyManager LM;
    private bool equilibrageOn;
    private int prevCops;
    private int prevRunners;
    [Tooltip("true = chasseur | false = runner")] public bool isCops;
    public string playerName;
    #endregion


    private void Start()
    {
        prevCops = 0;
        prevRunners = 0; 
    }

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

            LM = FindObjectOfType<LobbyManager>();

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
    }
}