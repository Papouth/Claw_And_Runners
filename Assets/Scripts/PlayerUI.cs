using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerUI : NetworkBehaviour
{
    private TeamSelection TS;
    private bool equilibrageOn;
    private int prevCops;
    private int prevRunners;

    private void Start()
    {
        prevCops = 0;
        prevRunners = 0; 
        TS = FindObjectOfType<TeamSelection>();
    }

    private void Update()
    {
        if (IsOwner && !equilibrageOn)
        {
            equilibrageOn = true;

            TS.Equilibrage();
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
        }
    }
}