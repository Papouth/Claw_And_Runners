using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerTeam : NetworkBehaviour
{
    public NetworkVariable<int> playerLife = new NetworkVariable<int>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Server);
    [SerializeField] public string team;

    private NetworkParameter NP;
    private TeamSelection teamSelection;


    private void Awake()
    {
        NP = NetworkManager.GetComponent<NetworkParameter>();
    }

    // Quand mon joueur apparait
    public override void OnNetworkSpawn()
    {
        teamSelection = FindObjectOfType<TeamSelection>();

        teamSelection.ShowHideUI();

        // On affiche l'UI de team selection avec les param�tres d�termin� par le nombre de joueurs via le lobbymanager
        NP.clientCount++;

        if (NP.clientCount == 1 || NP.clientCount == 3 || NP.clientCount == 5)
        {
            team = "red";
        }
        else if (NP.clientCount == 2 || NP.clientCount == 4 || NP.clientCount == 6)
        {
            team = "blue";
        }

        playerLife.Value = 500;

        Debug.Log(gameObject.name + " � " + playerLife.Value + " HP et fait parti de l'�quipe " + team);
    }
}