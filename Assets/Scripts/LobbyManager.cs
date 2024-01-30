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
    public string lobbyName = "Default Lobby";
    private string customLobbyName = "";
    public int maxPlayers;

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
    [SerializeField] private Image[] cadenas;
    [SerializeField] private Sprite[] cadenasBase;
    [SerializeField] private Sprite[] cadenasColor;

    [SerializeField] private Image[] imageNumber;
    [SerializeField] private Sprite[] spritesNumber;
    [SerializeField] private Sprite[] spritesNumberColor;

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
                teamSelection.parcName.text = insideLobbyName.text;
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

    // Arrow To increase and decrease

    //public void AddMorePlayer()
    //{
    //    basePlayerNumber = 4;
    //    increm++;
    //
    //    if (increm > 4) increm = 4;
    //
    //    basePlayerNumber = basePlayerNumber + increm;
    //    maxPlayersInLobbyText.text = basePlayerNumber.ToString();
    //    maxPlayers = basePlayerNumber;
    //}
    //
    //public void AddLessPlayer()
    //{
    //    basePlayerNumber = 4;
    //    increm--;
    //
    //    if (increm < -4) increm = -4;
    //
    //    basePlayerNumber = basePlayerNumber + increm;
    //    maxPlayersInLobbyText.text = basePlayerNumber.ToString();
    //    maxPlayers = basePlayerNumber;
    //}

    public void FourPlayer()
    {
        maxPlayers = 4;

        // On set la couleur
        imageNumber[0].sprite = spritesNumberColor[0];

        // On retire les autres couleurs
        for (int i = 1; i < imageNumber.Length; i++)
        {
            imageNumber[i].sprite = spritesNumber[i];
        }
    }

    public void FivePlayer()
    {
        maxPlayers = 5;

        // On set la couleur
        imageNumber[1].sprite = spritesNumberColor[1];

        // On retire les autres couleurs
        imageNumber[0].sprite = spritesNumber[0];

        for (int i = 2; i < imageNumber.Length; i++)
        {
            imageNumber[i].sprite = spritesNumber[i];
        }
    }

    public void SixPlayer()
    {
        maxPlayers = 6;

        // On set la couleur
        imageNumber[2].sprite = spritesNumberColor[2];

        // On retire les autres couleurs
        imageNumber[0].sprite = spritesNumber[0];
        imageNumber[1].sprite = spritesNumber[1];

        for (int i = 3; i < imageNumber.Length; i++)
        {
            imageNumber[i].sprite = spritesNumber[i];
        }
    }

    public void SevenPlayer()
    {
        maxPlayers = 7;

        // On set la couleur
        imageNumber[3].sprite = spritesNumberColor[3];

        // On retire les autres couleurs
        imageNumber[0].sprite = spritesNumber[0];
        imageNumber[1].sprite = spritesNumber[1];
        imageNumber[2].sprite = spritesNumber[2];
        imageNumber[4].sprite = spritesNumber[4];
    }

    public void EightPlayer()
    {
        maxPlayers = 8;

        // On retire les autres couleurs
        for (int i = 0; i < imageNumber.Length; i++)
        {
            imageNumber[i].sprite = spritesNumber[i];
        }

        // On set la couleur
        imageNumber[4].sprite = spritesNumberColor[4];
    }

    public void SpecialTestPlayer()
    {
        maxPlayers = 2;
        Debug.Log(maxPlayers + " est le nombre de joueurs maximum");
    }
    #endregion


    /// <summary>
    /// Rendre le lobby public
    /// </summary>
    public void SetPublicLobby()
    {
        stateLobby = false;

        // Set color
        cadenas[0].sprite = cadenasColor[0];

        // Reset other base color
        cadenas[1].sprite = cadenasBase[1];
    }

    /// <summary>
    /// Rendre le lobby privé
    /// </summary>
    public void SetPrivateLobby()
    {
        stateLobby = true;

        // Set color
        cadenas[1].sprite = cadenasColor[1];

        // Reset other base color
        cadenas[0].sprite = cadenasBase[0];
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

            Debug.Log("changement du nombre max de joueur lors de la création du lobby");

            hostLobby = joinedLobby;

            PrintPlayers(hostLobby);

            lobbyCodeDisplay.text = joinedLobby.LobbyCode;

            //Debug.Log("Created Lobby ! " + "Nom du Lobby : " + joinedLobby.Name + " | Nombre de Joueurs Max : " + joinedLobby.MaxPlayers + " | ID du Lobby : " + joinedLobby.Id + " | Code : " + joinedLobby.LobbyCode);

            teamSelection.createLobby = true;
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

                clone.transform.position = new Vector3(clone.transform.position.x - 300, clone.transform.position.y + 300, 0); // -550 & 400

                if (countDisplayer > 0)
                {
                    int multiplier = 0;

                    for (int i = 1; i < queryResponse.Results.Count; i++)
                    {
                        multiplier++;

                        clone.transform.position += new Vector3(0, -110 * multiplier, 0);
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
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            });

            PanelInsideLobby();

            PrintPlayers(joinedLobby);

            foreach (var item in cloneDisplayerObj)
            {
                Destroy(item);
            }
            cloneDisplayerObj.Clear();
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
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    #endregion


    private void UnlockStartButton()
    {
        if (IsLobbyHost())
        {
            startBttn.SetActive(true);
        }
    }

    #region Debug Player

    /// <summary>
    /// Actualise les infos du joueur
    /// </summary>
    /// <returns></returns>
    public Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }

    /// <summary>
    /// Affiche les différents noms des joueurs présent dans le lobby
    /// </summary>
    /// <param name="lobby"></param>
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
                    // Le joueur se trouve dans le lobby

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