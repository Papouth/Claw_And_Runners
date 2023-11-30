using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerUI : NetworkBehaviour
{
    private TeamSelection TS;
    private bool equilibrageOn;


    private void Start()
    {
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
            TS.UpdateRunnersNValue();
            TS.UpdateCopsNValue();

            //TS.UIMAJCopsName();
            //TS.UIMAJRunnersName();
        }
    }
}