using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private string lobbyName = "Default Lobby";
    private string customLobbyName = "";
    public int maxPlayers = 4;

    private float hearbeatTimer;
    private float lobbyUpdateTimer;
    private Lobby hostLobby;
    [HideInInspector] public Lobby joinedLobby;
    private string inputCode;
    public string playerName;
    [SerializeField] private TextMeshProUGUI IDText;
    [Tooltip("Nom Temporaire Joueur")][SerializeField] private TextMeshProUGUI namePlaceHolder;
    [Tooltip("Nom Temporaire Lobby")][SerializeField] private TextMeshProUGUI lobbyNamePlaceHolder;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeDisplay;

    [Header("Lobby Parameters")]
    private bool stateLobby;
    [SerializeField] private TextMeshProUGUI lobbyStateText;
    [SerializeField] private TextMeshProUGUI maxPlayersInLobbyText;
    private int increm;
    private int basePlayerNumber = 4;

    [Header("Inside Lobby")]
    [SerializeField] private TextMeshProUGUI insideLobbyName;
    [SerializeField] private TextMeshProUGUI actualPlayersInsideLobby;
    [SerializeField] private TextMeshProUGUI maxPlayersInsideLobby;
    [SerializeField] private TextMeshProUGUI[] namesPlayersInsideLobby;
    private int numP;

    [Header("UI Panels")]
    [SerializeField] private GameObject lobbyMenu;
    [SerializeField] private GameObject createLobbyMenu;
    [SerializeField] private GameObject insideLobbyMenu;

    [Header("Lobby Listing")]
    [SerializeField] private List<string> lobbyNamesList = new List<string>();
    [SerializeField] private GameObject bttnLobbyDisplayer;
    [SerializeField] private Transform canvas;
    private LobbyDisplayer lobbyDisplayer;
    private int countDisplayer;
    private List<GameObject> cloneDisplayerObj = new List<GameObject>();
    private Button buttonLobbyDisplay;

    [Header("In Game")]
    [HideInInspector] public bool gameStarted;

    // [Header("Team Selection")]
    // private int copsLimit;
    // private int copsN;
    // [SerializeField] private TextMeshProUGUI copsNumberTxt;
    // [SerializeField] private TextMeshProUGUI copsMaxNumberTxt;
    // [HideInInspector][SerializeField] private List<string> copsPlayerNameTxt;
    // [SerializeField] private List<TextMeshProUGUI> copsPlayerNameTMPro;
    // 
    // private int runnersLimit;
    // private int runnersN;
    // [SerializeField] private TextMeshProUGUI runnersNumberTxt;
    // [SerializeField] private TextMeshProUGUI runnersMaxNumberTxt;
    // [HideInInspector][SerializeField] private List<string> runnersPlayerNameTxt;
    // [SerializeField] private List<TextMeshProUGUI> runnersPlayerNameTMPro;
    // 
    // private bool alreadyCop;
    // private bool alreadyRunner;

    [SerializeField] private GameObject startBttn;
    [SerializeField] private GameObject camUIMenu;
    [SerializeField] private TeamSelection teamSelection;
    #endregion

    #region Built In Methods
    private void Awake()
    {
        startBttn.SetActive(false);
        playerName = "FunkyPlayer" + Random.Range(10, 99);
        namePlaceHolder.text = playerName;
        increm = 0;
        maxPlayersInLobbyText.text = basePlayerNumber.ToString();

        foreach (var item in namesPlayersInsideLobby)
        {
            item.text = "";
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            IDText.text = AuthenticationService.Instance.PlayerId.ToString();
            //Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //Debug.Log(playerName);

        lobbyMenu.SetActive(true);
        createLobbyMenu.SetActive(false);
        insideLobbyMenu.SetActive(false);

        teamSelection = FindObjectOfType<TeamSelection>();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();

        HandleLobbyPollForUpdates();
    }
    #endregion


    #region Customs Methods

    #region Panels
    public void PanelCreationLobby()
    {
        SetLobbyNameDefault();
        createLobbyMenu.SetActive(true);
        lobbyMenu.SetActive(false);
        insideLobbyMenu.SetActive(false);
    }

    public void PanelInsideLobby()
    {
        insideLobbyMenu.SetActive(true);
        createLobbyMenu.SetActive(false);
        lobbyMenu.SetActive(false);
        startBttn.SetActive(false);
    }

    public void PanelMenuLobby()
    {
        lobbyMenu.SetActive(true);
        createLobbyMenu.SetActive(false);
        insideLobbyMenu.SetActive(false);
    }
    #endregion


    #region Names Changing
    /// <summary>
    /// Permet au joueur de changer le nom du lobby
    /// </summary>
    /// <param name="lobbyCustomName"></param>
    public void ChooseLobbyName(string lobbyCustomName)
    {
        lobbyName = lobbyCustomName;
    }

    /// <summary>
    /// Permet de set le nom du lobby
    /// </summary>
    public void SetLobbyNameDefault()
    {
        customLobbyName = lobbyNamesList[Random.Range(0, lobbyNamesList.Count)];
        lobbyNamePlaceHolder.text = customLobbyName;
        lobbyName = customLobbyName;
    }

    /// <summary>
    /// Permet au joueur de personnaliser son nom
    /// </summary>
    public void ChooseName(string customName)
    {
        playerName = customName;
    }
    #endregion


    /// <summary>
    /// Permet de laisser en vie le salon créé
    /// </summary>
    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            hearbeatTimer -= Time.deltaTime;
            if (hearbeatTimer < 0f)
            {
                float hearbeatTimerMax = 15f;
                hearbeatTimer = hearbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    /// <summary>
    /// Permet d'actualiser le lobby et les infos qu'il contient
    /// </summary>
    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            // Lobby Data
            if (!gameStarted)
            {
                actualPlayersInsideLobby.text = joinedLobby.Players.Count.ToString();
                maxPlayersInsideLobby.text = joinedLobby.MaxPlayers.ToString();
                insideLobbyName.text = joinedLobby.Name;
                lobbyCodeDisplay.text = joinedLobby.LobbyCode;

                //foreach (var player in joinedLobby.Players)
                //{
                //
                //    //copsNumberTxt.text = copsN.ToString();
                //    //runnersNumberTxt.text = runnersN.ToString();
                //}

                if (joinedLobby.Players.Count == maxPlayers)
                {
                    UnlockStartButton();
                }
            }

            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                if (!gameStarted) PrintPlayers(joinedLobby);

                if (joinedLobby.Data["KEY_START_GAME"].Value != "0")
                {
                    // Start Game
                    if (!IsLobbyHost())
                    {
                        // Lobby Host already joined Relay
                        RelayManager.Instance.JoinRelay(joinedLobby.Data["KEY_START_GAME"].Value);
                    }

                    joinedLobby = null;
                    hostLobby = null;
                }
            }
        }
    }


    #region Lobby Creation & Joining

    #region Set maximum players in lobby
    // 4 to 8 players

    public void AddMorePlayer()
    {
        basePlayerNumber = 4;
        increm++;

        if (increm > 4) increm = 4;

        basePlayerNumber = basePlayerNumber + increm;
        maxPlayersInLobbyText.text = basePlayerNumber.ToString();
        maxPlayers = basePlayerNumber;
    }

    public void AddLessPlayer()
    {
        basePlayerNumber = 4;
        increm--;

        if (increm < -4) increm = -4;

        basePlayerNumber = basePlayerNumber + increm;
        maxPlayersInLobbyText.text = basePlayerNumber.ToString();
        maxPlayers = basePlayerNumber;
    }

    public void SpecialTestPlayer()
    {
        maxPlayersInLobbyText.text = "2";
        maxPlayers = 2;
    }
    #endregion

    /// <summary>
    /// Mettre un lobby en public ou en privé
    /// </summary>
    public void ChangeLobbyState()
    {
        stateLobby = !stateLobby;

        if (stateLobby)
        {
            lobbyStateText.text = "PRIVATE";
        }
        else if (!stateLobby)
        {
            lobbyStateText.text = "PUBLIC";
        }
    }

    /// <summary>
    /// Créer un lobby
    /// </summary>
    public async void CreateLobby()
    {
        try
        {
            // Permet de changer les options du lobby avant de le créer
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                // Mettre en true pour rejoindre par code | false pour être en publique
                IsPrivate = stateLobby,

                Player = new Player
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                    }
                },

                Data = new Dictionary<string, DataObject> {
                    {"KEY_START_GAME", new DataObject(DataObject.VisibilityOptions.Member, "0")}
                }
            };

            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = joinedLobby;

            PrintPlayers(hostLobby);

            lobbyCodeDisplay.text = joinedLobby.LobbyCode;

            //Debug.Log("Created Lobby ! " + "Nom du Lobby : " + joinedLobby.Name + " | Nombre de Joueurs Max : " + joinedLobby.MaxPlayers + " | ID du Lobby : " + joinedLobby.Id + " | Code : " + joinedLobby.LobbyCode);

            teamSelection.createLobby = true;

            //InitTeamSelection();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Liste les lobbys existants
    /// </summary>
    public async void ListLobbies()
    {
        try
        {
            foreach (var item in cloneDisplayerObj)
            {
                Destroy(item);
            }
            cloneDisplayerObj.Clear();

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            //Debug.Log("Lobbies found : " + queryResponse.Results.Count);

            countDisplayer = -1;

            foreach (Lobby lobby in queryResponse.Results)
            {
                countDisplayer++;

                //Debug.Log(lobby.Name + " | " + lobby.MaxPlayers);

                var clone = Instantiate(bttnLobbyDisplayer, lobbyMenu.transform);
                cloneDisplayerObj.Add(clone);

                clone.transform.position = new Vector3(clone.transform.position.x - 550, clone.transform.position.y + 400, 0);

                if (countDisplayer > 0)
                {
                    int multiplier = 0;

                    for (int i = 1; i < queryResponse.Results.Count; i++)
                    {
                        multiplier++;

                        clone.transform.position += new Vector3(0, -100 * multiplier, 0);
                    }
                }

                lobbyDisplayer = clone.GetComponent<LobbyDisplayer>();

                lobbyDisplayer.numPlayerInLobby.text = lobby.Players.Count.ToString();
                lobbyDisplayer.maxNumPlayerInLobby.text = lobby.MaxPlayers.ToString();
                lobbyDisplayer.activeLobbyName.text = lobby.Name.ToString();

                buttonLobbyDisplay = clone.GetComponent<Button>();

                buttonLobbyDisplay.onClick.AddListener(() => { JoinLobbyOnClick(lobby); });
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Rejoindre un lobby publique via un boutton
    /// </summary>
    /// <param name="lobby"></param>
    public async void JoinLobbyOnClick(Lobby lobby)
    {
        try
        {
            Player player = GetPlayer();

            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
            {
                Player = player
            });

            PanelInsideLobby();

            PrintPlayers(joinedLobby);

            foreach (var item in cloneDisplayerObj)
            {
                Destroy(item);
            }
            cloneDisplayerObj.Clear();

            //InitTeamSelection();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Rejoindre un lobby existant via un code de lobby
    /// </summary>
    /// <param name="lobbyCode"></param>
    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            inputCode = lobbyCode;

            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(inputCode, joinLobbyByCodeOptions);

            Debug.Log("Joined Lobby with code " + inputCode);

            PanelInsideLobby();

            PrintPlayers(joinedLobby);

            //InitTeamSelection();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    #endregion


    #region Team Selection
    // private void InitTeamSelection()
    // {
    //     for (int i = 0; i < copsPlayerNameTMPro.Capacity; i++)
    //     {
    //         copsPlayerNameTMPro[i].text = "";
    //         copsPlayerNameTxt.Add(i.ToString());
    //     }
    // 
    //     for (int i = 0; i < runnersPlayerNameTMPro.Capacity; i++)
    //     {
    //         runnersPlayerNameTMPro[i].text = "";
    //         runnersPlayerNameTxt.Add(i.ToString());
    //     }
    // }
    // 
    // /// <summary>
    // /// Rejoindre les policiers
    // /// </summary>
    // public void JoinCops()
    // {
    //     for (int i = 0; i < copsPlayerNameTxt.Count; i++)
    //     {
    //         if (copsPlayerNameTxt[i].Contains(playerName))
    //         {
    //             alreadyCop = true;
    //         }
    //     }
    // 
    //     // S'il y a de la place et que je ne suis pas encore dans cette équipe
    //     if (copsN < copsLimit && !alreadyCop && !alreadyRunner)
    //     {
    //         copsPlayerNameTxt[copsN] = playerName;
    //         copsPlayerNameTMPro[copsN].text = copsPlayerNameTxt[copsN];
    // 
    //         copsN++;
    //         copsNumberTxt.text = copsN.ToString();
    // 
    //         alreadyCop = true;
    //     }
    // 
    //     // Si je suis déjà dans l'équipe des courreurs et qu'il y a de la place chez les policiers
    //     if (alreadyRunner && copsN < copsLimit && !alreadyCop)
    //     {
    //         alreadyRunner = false;
    //         alreadyCop = true;
    // 
    //         // On ajoute notre nom à la liste des policiers
    //         copsPlayerNameTxt[copsN] = playerName;
    //         copsPlayerNameTMPro[copsN].text = copsPlayerNameTxt[copsN];
    // 
    //         copsN++;
    // 
    //         // On retire de la liste des courreurs notre nom
    //         int index = runnersPlayerNameTxt.IndexOf(playerName);
    //         runnersPlayerNameTxt[index] = "";
    //         runnersPlayerNameTMPro[index].text = "";
    // 
    //         runnersN--;
    // 
    //         // Nombre de policier et de courreur
    //         runnersNumberTxt.text = runnersN.ToString();
    //         copsNumberTxt.text = copsN.ToString();
    //     }
    // }
    // 
    // /// <summary>
    // /// Rejoindre les courreurs
    // /// </summary>
    // public void JoinRunners()
    // {
    //     for (int i = 0; i < runnersPlayerNameTxt.Count; i++)
    //     {
    //         if (runnersPlayerNameTxt[i].Contains(playerName))
    //         {
    //             alreadyRunner = true;
    //         }
    //     }
    // 
    //     // S'il y a de la place et que je ne suis pas encore dans cette équipe
    //     if (runnersN < runnersLimit && !alreadyRunner && !alreadyCop)
    //     {
    //         runnersPlayerNameTxt[runnersN] = playerName;
    //         runnersPlayerNameTMPro[runnersN].text = runnersPlayerNameTxt[runnersN];
    // 
    //         runnersN++;
    //         runnersNumberTxt.text = runnersN.ToString();
    // 
    //         alreadyRunner = true;
    //     }
    // 
    //     // Si je suis déjà dans l'équipe des policiers et qu'il y a de la place chez les courreurs
    //     if (alreadyCop && runnersN < runnersLimit && !alreadyRunner)
    //     {
    //         alreadyCop = false;
    //         alreadyRunner = true;
    // 
    //         // On ajoute notre nom à la liste des courreurs
    //         runnersPlayerNameTxt[runnersN] = playerName;
    //         runnersPlayerNameTMPro[runnersN].text = runnersPlayerNameTxt[runnersN];
    // 
    //         runnersN++;
    // 
    //         // On retire de la liste des policiers notre nom
    //         int index = copsPlayerNameTxt.IndexOf(playerName);
    //         copsPlayerNameTxt[index] = "";
    //         copsPlayerNameTMPro[index].text = "";
    // 
    //         copsN--;
    // 
    //         // Nombre de policier et de courreur
    //         copsNumberTxt.text = copsN.ToString();
    //         runnersNumberTxt.text = runnersN.ToString();
    //     }
    // }
    // 
    // private void Equilibrage()
    // {
    //     switch (maxPlayers)
    //     {
    //         case 8:
    //             Debug.Log("Il y a 2 Cops | 6 Runners");
    //             copsLimit = 2;
    //             runnersLimit = 6;
    //             break;
    //         case 7:
    //             Debug.Log("Il y a 2 Cops | 5 Runners");
    //             copsLimit = 2;
    //             runnersLimit = 5;
    //             break;
    //         case 6:
    //             Debug.Log("Il y a 2 Cops | 4 Runners");
    //             copsLimit = 2;
    //             runnersLimit = 4;
    //             break;
    //         case 5:
    //             Debug.Log("Il y a 1 Cops | 4 Runners");
    //             copsLimit = 1;
    //             runnersLimit = 4;
    //             break;
    //         case 4:
    //             Debug.Log("Il y a 1 Cops | 3 Runners");
    //             copsLimit = 1;
    //             runnersLimit = 3;
    //             break;
    //         case 2:
    //             Debug.Log("Test 1 joueur de chaque");
    //             copsLimit = 1;
    //             runnersLimit = 1;
    //             break;
    //     }
    // 
    //     copsMaxNumberTxt.text = copsLimit.ToString();
    //     runnersMaxNumberTxt.text = runnersLimit.ToString();
    // }
    #endregion

    private void UnlockStartButton()
    {
        if (IsLobbyHost())
        {
            startBttn.SetActive(true);
        }
    }

    #region Debug Player
    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    private void PrintPlayers(Lobby lobby)
    {
        //Debug.Log("Players in Lobby : " + lobby.Name);

        for (int i = 0; i < namesPlayersInsideLobby.Length; i++)
        {
            namesPlayersInsideLobby[i].text = "";
        }

        foreach (Player player in lobby.Players)
        {
            namesPlayersInsideLobby[numP].text = player.Data["PlayerName"].Value.ToString();

            //Debug.Log(player.Id + " | " + player.Data["PlayerName"].Value);

            numP++;
        }

        numP = 0;
    }
    #endregion

    /// <summary>
    /// Permet de quitter le salon
    /// </summary>
    public async void LeaveLobby()
    {
        try
        {
            // for (int i = 0; i < runnersPlayerNameTxt.Count; i++)
            // {
            //     if (runnersPlayerNameTxt[i].Contains(playerName))
            //     {
            //         runnersN--;
            //         runnersNumberTxt.text = runnersN.ToString();
            //     }
            // }
            // 
            // alreadyRunner = false;
            // runnersPlayerNameTxt[runnersN] = "";
            // runnersPlayerNameTMPro[runnersN].text = "";
            // 
            // for (int i = 0; i < copsPlayerNameTxt.Count; i++)
            // {
            //     if (copsPlayerNameTxt[i].Contains(playerName))
            //     {
            //         copsN--;
            //         copsNumberTxt.text = copsN.ToString();
            //     }
            // }
            // 
            // alreadyCop = false;
            // copsPlayerNameTxt[copsN] = "";
            // copsPlayerNameTMPro[copsN].text = "";

            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

            joinedLobby = null;

            PanelMenuLobby();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Permet de supprimer le lobby
    /// </summary>
    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

            joinedLobby = null;
            hostLobby = null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // Le joueur ce trouve dans le lobby
                    return true;
                }
            }
        }
        return false;
    }

    public async void StartGame()
    {
        if (IsLobbyHost())
        {
            try
            {
                Debug.Log("StartGame");

                string relayCode = await RelayManager.Instance.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject> {
                        {"KEY_START_GAME", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                joinedLobby = lobby;

                gameStarted = true;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
    #endregion
}