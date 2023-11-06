using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private string lobbyName = "Default Lobby";
    private string customLobbyName = "";
    [SerializeField] private int maxPlayers = 4;

    private float hearbeatTimer;
    private float lobbyUpdateTimer;
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private string inputCode;
    private string playerName;
    [SerializeField] private TextMeshProUGUI IDText;
    [Tooltip("Nom Temporaire Joueur")][SerializeField] private TextMeshProUGUI namePlaceHolder;
    [Tooltip("Nom Temporaire Lobby")][SerializeField] private TextMeshProUGUI lobbyNamePlaceHolder;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeDisplay;

    [Header("UI Panels")]
    [SerializeField] private GameObject lobbyMenu;
    [SerializeField] private GameObject createLobbyMenu;
    [SerializeField] private GameObject insideLobbyMenu;

    [SerializeField] private List<string> lobbyNamesList = new List<string>();
    #endregion

    #region Built In Methods
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            IDText.text = AuthenticationService.Instance.PlayerId.ToString();
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "FunkyPlayer" + Random.Range(10, 99);
        namePlaceHolder.text = playerName;

        Debug.Log(playerName);


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
    }

    public void PanelMenuLobby()
    {
        lobbyMenu.SetActive(true);
        createLobbyMenu.SetActive(false);
        insideLobbyMenu.SetActive(false);
    }

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
        UpdatePlayerName(customName);
    }

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
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
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
                // Mettre en true pour rejoindre par code
                IsPrivate = false,

                Player = new Player
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                    }
                }
            };

            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = joinedLobby;

            PrintPlayers(hostLobby);

            IDText.text = joinedLobby.LobbyCode;
            lobbyCodeDisplay.text =  joinedLobby.LobbyCode;

            Debug.Log("Created Lobby ! " + "Nom du Lobby : " + joinedLobby.Name + " | Nombre de Joueurs Max : " + joinedLobby.MaxPlayers + " | ID du Lobby : " + joinedLobby.Id + " | Code : " + joinedLobby.LobbyCode);
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
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found : " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " | " + lobby.MaxPlayers);
            }
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

            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
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
        Debug.Log("Players in Lobby : " + lobby.Name);

        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " | " + player.Data["PlayerName"].Value);
        }
    }

    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject> {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
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
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    #endregion
}