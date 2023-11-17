using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;

public class TeamSelection : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject UITeamSelection;

    // // Team Selection Max Player Number
    // public NetworkVariable<int> copsLimit = new NetworkVariable<int>();
    // public NetworkVariable<int> runnersLimit = new NetworkVariable<int>();
    // 
    // // Team Selection Actual Player Number
    // public NetworkVariable<int> copsN = new NetworkVariable<int>();
    // public NetworkVariable<int> runnersN = new NetworkVariable<int>();
    // 
    // // Noms des joueurs de chaque équipe
    // public string[] copsPlayerNameTxt;
    // public string[] runnersPlayerNameTxt;


    [Header("Team Selection")]
    private int copsLimit;
    private int copsN;
    [SerializeField] private TextMeshProUGUI copsNumberTxt;
    [SerializeField] private TextMeshProUGUI copsMaxNumberTxt;
    [HideInInspector][SerializeField] private List<string> copsPlayerNameTxt;
    [SerializeField] private List<TextMeshProUGUI> copsPlayerNameTMPro;

    private int runnersLimit;
    private int runnersN;
    [SerializeField] private TextMeshProUGUI runnersNumberTxt;
    [SerializeField] private TextMeshProUGUI runnersMaxNumberTxt;
    [HideInInspector][SerializeField] private List<string> runnersPlayerNameTxt;
    [SerializeField] private List<TextMeshProUGUI> runnersPlayerNameTMPro;

    private bool alreadyCop;
    private bool alreadyRunner;


    private bool displayUI;
    [SerializeField] private LobbyManager LM;
    #endregion


    //  // Quand mon joueur apparait
    //  public override void OnNetworkSpawn()
    //  {
    //      // On affiche l'UI de team selection avec les paramètres déterminé par le nombre de joueurs via le lobbymanager
    //  
    //  
    //  }

    #region Built Int Methods
    private void Awake()
    {
        UITeamSelection.SetActive(false);
        LM = GetComponent<LobbyManager>();
    }

    private void Update()
    {
        ShowHideUI();
    }
    #endregion

    #region Customs Methods
    private void ShowHideUI()
    {
        if (!displayUI)
        {
            displayUI = true;

            UITeamSelection.SetActive(true);
        }
    }


    #region Team Selection
    private void InitTeamSelection()
    {
        for (int i = 0; i < copsPlayerNameTMPro.Capacity; i++)
        {
            copsPlayerNameTMPro[i].text = "";
            copsPlayerNameTxt.Add(i.ToString());
        }

        for (int i = 0; i < runnersPlayerNameTMPro.Capacity; i++)
        {
            runnersPlayerNameTMPro[i].text = "";
            runnersPlayerNameTxt.Add(i.ToString());
        }
    }

    /// <summary>
    /// Rejoindre les policiers
    /// </summary>
    public void JoinCops()
    {
        for (int i = 0; i < copsPlayerNameTxt.Count; i++)
        {
            if (copsPlayerNameTxt[i].Contains(LM.playerName))
            {
                alreadyCop = true;
            }
        }

        // S'il y a de la place et que je ne suis pas encore dans cette équipe
        if (copsN < copsLimit && !alreadyCop && !alreadyRunner)
        {
            copsPlayerNameTxt[copsN] = LM.playerName;
            copsPlayerNameTMPro[copsN].text = copsPlayerNameTxt[copsN];

            copsN++;
            copsNumberTxt.text = copsN.ToString();

            alreadyCop = true;
        }

        // Si je suis déjà dans l'équipe des courreurs et qu'il y a de la place chez les policiers
        if (alreadyRunner && copsN < copsLimit && !alreadyCop)
        {
            alreadyRunner = false;
            alreadyCop = true;

            // On ajoute notre nom à la liste des policiers
            copsPlayerNameTxt[copsN] = LM.playerName;
            copsPlayerNameTMPro[copsN].text = copsPlayerNameTxt[copsN];

            copsN++;

            // On retire de la liste des courreurs notre nom
            int index = runnersPlayerNameTxt.IndexOf(LM.playerName);
            runnersPlayerNameTxt[index] = "";
            runnersPlayerNameTMPro[index].text = "";

            runnersN--;

            // Nombre de policier et de courreur
            runnersNumberTxt.text = runnersN.ToString();
            copsNumberTxt.text = copsN.ToString();
        }
    }

    /// <summary>
    /// Rejoindre les courreurs
    /// </summary>
    public void JoinRunners()
    {
        for (int i = 0; i < runnersPlayerNameTxt.Count; i++)
        {
            if (runnersPlayerNameTxt[i].Contains(LM.playerName))
            {
                alreadyRunner = true;
            }
        }

        // S'il y a de la place et que je ne suis pas encore dans cette équipe
        if (runnersN < runnersLimit && !alreadyRunner && !alreadyCop)
        {
            runnersPlayerNameTxt[runnersN] = LM.playerName;
            runnersPlayerNameTMPro[runnersN].text = runnersPlayerNameTxt[runnersN];

            runnersN++;
            runnersNumberTxt.text = runnersN.ToString();

            alreadyRunner = true;
        }

        // Si je suis déjà dans l'équipe des policiers et qu'il y a de la place chez les courreurs
        if (alreadyCop && runnersN < runnersLimit && !alreadyRunner)
        {
            alreadyCop = false;
            alreadyRunner = true;

            // On ajoute notre nom à la liste des courreurs
            runnersPlayerNameTxt[runnersN] = LM.playerName;
            runnersPlayerNameTMPro[runnersN].text = runnersPlayerNameTxt[runnersN];

            runnersN++;

            // On retire de la liste des policiers notre nom
            int index = copsPlayerNameTxt.IndexOf(LM.playerName);
            copsPlayerNameTxt[index] = "";
            copsPlayerNameTMPro[index].text = "";

            copsN--;

            // Nombre de policier et de courreur
            copsNumberTxt.text = copsN.ToString();
            runnersNumberTxt.text = runnersN.ToString();
        }
    }

    private void Equilibrage()
    {
        switch (LM.maxPlayers)
        {
            case 8:
                Debug.Log("Il y a 2 Cops | 6 Runners");
                copsLimit = 2;
                runnersLimit = 6;
                break;
            case 7:
                Debug.Log("Il y a 2 Cops | 5 Runners");
                copsLimit = 2;
                runnersLimit = 5;
                break;
            case 6:
                Debug.Log("Il y a 2 Cops | 4 Runners");
                copsLimit = 2;
                runnersLimit = 4;
                break;
            case 5:
                Debug.Log("Il y a 1 Cops | 4 Runners");
                copsLimit = 1;
                runnersLimit = 4;
                break;
            case 4:
                Debug.Log("Il y a 1 Cops | 3 Runners");
                copsLimit = 1;
                runnersLimit = 3;
                break;
            case 2:
                Debug.Log("Test 1 joueur de chaque");
                copsLimit = 1;
                runnersLimit = 1;
                break;
        }

        copsMaxNumberTxt.text = copsLimit.ToString();
        runnersMaxNumberTxt.text = runnersLimit.ToString();
    }
    #endregion


    /// <summary>
    /// Permet de quitter le salon
    /// </summary>
    public async void LeaveLobby()
    {
        try
        {
            for (int i = 0; i < runnersPlayerNameTxt.Count; i++)
            {
                if (runnersPlayerNameTxt[i].Contains(LM.playerName))
                {
                    runnersN--;
                    runnersNumberTxt.text = runnersN.ToString();
                }
            }

            alreadyRunner = false;
            runnersPlayerNameTxt[runnersN] = "";
            runnersPlayerNameTMPro[runnersN].text = "";

            for (int i = 0; i < copsPlayerNameTxt.Count; i++)
            {
                if (copsPlayerNameTxt[i].Contains(LM.playerName))
                {
                    copsN--;
                    copsNumberTxt.text = copsN.ToString();
                }
            }

            alreadyCop = false;
            copsPlayerNameTxt[copsN] = "";
            copsPlayerNameTMPro[copsN].text = "";

            await LobbyService.Instance.RemovePlayerAsync(LM.joinedLobby.Id, AuthenticationService.Instance.PlayerId);

            LM.joinedLobby = null;

            LM.PanelMenuLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    #endregion
}