using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Netcode;
using Unity.Collections;


public class TeamSelection : NetworkBehaviour
{
    #region Variables
    [SerializeField] private GameObject UITeamSelection;

    // Team Selection Max Player Number
    public NetworkVariable<int> copsLimit;
    public NetworkVariable<int> runnersLimit;

    // Team Selection Actual Player Number
    public NetworkVariable<int> copsN;
    public NetworkVariable<int> runnersN;

    // Noms des joueurs de chaque �quipe
    public List<string> copsPlayerNameTxt;
    public List<string> runnersPlayerNameTxt;

    public NetworkList<FixedString32Bytes> runnersNamesList;
    public NetworkList<FixedString32Bytes> copsNamesList;

    [Header("Team Selection")]
    [SerializeField] private TextMeshProUGUI copsNumberTxt;
    [SerializeField] private TextMeshProUGUI copsMaxNumberTxt;

    [SerializeField] private List<TextMeshProUGUI> copsPlayerNameTMPro;

    [SerializeField] private TextMeshProUGUI runnersNumberTxt;
    [SerializeField] private TextMeshProUGUI runnersMaxNumberTxt;

    [SerializeField] private List<GameObject> copsFondText;
    [SerializeField] private List<GameObject> runnersFondText;

    [SerializeField] private List<TextMeshProUGUI> runnersPlayerNameTMPro;
    public TextMeshProUGUI parcName;

    private bool alreadyCop;
    private bool alreadyRunner;

    [SerializeField] private LobbyManager LM;
    [HideInInspector] public bool createLobby;
    private bool gameFullyStarted;
    private bool cursorState;

    [HideInInspector] public bool requireNameUpdate;

    [Header("Timer")]
    [SerializeField] private GameObject panelTimer;
    [SerializeField] private TextMeshProUGUI timer;
    private float timerToBegin = 5f;
    public bool readySelection;
    private bool showTimer;

    public bool nameIsSetup;
    private bool tagSetup;
    private SessionManager SM;
    #endregion


    #region Built In Methods
    private void Awake()
    {
        copsLimit = new NetworkVariable<int>();
        runnersLimit = new NetworkVariable<int>();

        copsN = new NetworkVariable<int>();
        runnersN = new NetworkVariable<int>();

        runnersNamesList = new NetworkList<FixedString32Bytes>();
        copsNamesList = new NetworkList<FixedString32Bytes>();
    }

    private void Start()
    {
        LM = FindObjectOfType<LobbyManager>();
        SM = gameObject.GetComponent<SessionManager>();

        UITeamSelection.SetActive(false);
        panelTimer.SetActive(false);


        for (int i = 0; i < copsFondText.Count; i++)
        {
            copsFondText[i].SetActive(false);
        }

        for (int i = 0; i < runnersFondText.Count; i++)
        {
            runnersFondText[i].SetActive(false);
        }
    }

    private void Update()
    {
        CursorModification();

        TimerToBegin();

        if (nameIsSetup && !tagSetup)
        {
            tagSetup = true;
            // ICI ON FAIT APPARAITRE CHAQUE JOUEUR A LA POSITION DE DEPART + ATTRIBUTION DES SETTINGS
            SM.AttributionTag();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            copsN.Value = 0;
            runnersN.Value = 0;
        }

        copsN.OnValueChanged += OncopsNChanged;
        runnersN.OnValueChanged += OnrunnersNChanged;

        copsLimit.OnValueChanged += OncopsLimitChanged;
        runnersLimit.OnValueChanged += OnrunnersLimitChanged;

        InitTeamSelection();
    }
    #endregion


    #region Customs Methods

    /// <summary>
    /// Permet de lancer la partie apr�s le choix des �quipes
    /// </summary>
    private void TimerToBegin()
    {
        // Si la s�lection d'�quipe n'est pas encore faite et qu'il y a autant de policiers que la limite ainsi qu'autant de courreurs que la limite alors on lance la partie
        if (!readySelection && copsN.Value == copsLimit.Value && runnersN.Value == runnersLimit.Value && showTimer)
        {
            //Debug.Log("timer start");
            panelTimer.SetActive(true);

            timerToBegin -= Time.deltaTime;
            timer.text = timerToBegin.ToString("0");
            if (timerToBegin <= 0f)
            {
                readySelection = true;

                


                // On retire l'UI de s�lection d'�quipe
                UITeamSelection.SetActive(false);
            }
        }
    }

    #region Netcode Cops
    private void OncopsNChanged(int previous, int current)
    {
        //Debug.Log("Changement de variable copsN, pr�c�dente valeur : " + previous + " |  nouvelle valeur : " + current);
    }

    private void OncopsLimitChanged(int previous, int current)
    {
        //Debug.Log("Changement de variable copsLimit, pr�c�dente valeur : " + previous + " |  nouvelle valeur : " + current);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MorecopsNValueServerRpc()
    {
        copsN.Value++;

        //Debug.Log("+ de flic");

        UpdateCopsNValue();
    }

    public void UpdateCopsNValue()
    {
        copsNumberTxt.text = copsN.Value.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerNameCopsServerRpc(FixedString32Bytes playerNameCops)
    {
        copsNamesList.Add(playerNameCops);
        //Debug.Log("cops noms : " + copsNamesList[0]);
    }

    public void PlayerNameCops(string name)
    {
        PlayerNameCopsServerRpc(new FixedString32Bytes(name));
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemovePlayerNameCopsServerRpc(FixedString32Bytes playerNameCops)
    {
        int index = copsNamesList.IndexOf(playerNameCops);
        copsNamesList[index] = "";

        copsPlayerNameTMPro[index].text = copsNamesList[index].ToString();
        copsNamesList.RemoveAt(index);

        requireNameUpdate = true;
    }

    public void RemovePlayerNameCops(string name)
    {
        RemovePlayerNameCopsServerRpc(new FixedString32Bytes(name));
    }

    [ServerRpc(RequireOwnership = false)]
    public void LesscopsNValueServerRpc()
    {
        copsN.Value--;

        //Debug.Log("- de flic");

        UpdateCopsNValue();
    }

    [ServerRpc]
    public void copsLimitValueServerRpc(int newValue)
    {
        copsLimit.Value = newValue;
        //Debug.Log("nouvelle limite de flic");
    }
    #endregion

    #region Netcode Runners
    private void OnrunnersNChanged(int previous, int current)
    {
        //Debug.Log("Changement de variable runnersN, pr�c�dente valeur : " + previous + " |  nouvelle valeur : " + current);
    }

    private void OnrunnersLimitChanged(int previous, int current)
    {
        //Debug.Log("Changement de variable runnersLimit, pr�c�dente valeur : " + previous + " |  nouvelle valeur : " + current);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MorerunnersNValueServerRpc()
    {
        runnersN.Value++;

        //Debug.Log("+ de runners");

        UpdateRunnersNValue();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerNameRunnersServerRpc(FixedString32Bytes playerNameRunners)
    {
        runnersNamesList.Add(playerNameRunners);
        //Debug.Log("runners noms : " + runnersNamesList[0]);
    }

    public void PlayerNameRunners(string name)
    {
        PlayerNameRunnersServerRpc(new FixedString32Bytes(name));
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemovePlayerNameRunnersServerRpc(FixedString32Bytes playerNameRunners)
    {
        int index = runnersNamesList.IndexOf(playerNameRunners);
        runnersNamesList[index] = "";

        runnersPlayerNameTMPro[index].text = runnersNamesList[index].ToString();
        runnersNamesList.RemoveAt(index);

        requireNameUpdate = true;
    }

    public void RemovePlayerNameRunners(string name)
    {
        RemovePlayerNameRunnersServerRpc(new FixedString32Bytes(name));
    }

    public void UpdateSelectionNames()
    {
        // Cops Team Update
        for (int i = 0; i < copsNamesList.Count; i++)
        {
            copsPlayerNameTMPro[i].text = copsNamesList[i].ToString();
        }

        if (copsNamesList.Count == 0)
        {
            copsPlayerNameTMPro[0].text = "";
        }

        for (int i = 0; i < runnersNamesList.Count; i++)
        {
            runnersPlayerNameTMPro[i].text = runnersNamesList[i].ToString();
        }

        if (runnersNamesList.Count == 0)
        {
            runnersPlayerNameTMPro[0].text = "";
        }
    }

    public void UpdateRunnersNValue()
    {
        runnersNumberTxt.text = runnersN.Value.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void LessrunnersNValueServerRpc()
    {
        runnersN.Value--;

        //Debug.Log("- de runners");

        UpdateRunnersNValue();
    }

    [ServerRpc]
    public void runnersLimitValueServerRpc(int newValue)
    {
        runnersLimit.Value = newValue;
        //Debug.Log("nouvelle limite de runner");
    }
    #endregion

    private void CursorModification()
    {
        if (gameFullyStarted && !cursorState)
        {
            //Debug.Log("game started true");
            cursorState = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (createLobby && !cursorState)
        {
            //Debug.Log("lobby creation");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void ShowHideUI()
    {
        UITeamSelection.SetActive(true);
    }

    #region Team Selection
    public void InitTeamSelection()
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

        // S'il y a de la place et que je ne suis pas encore dans cette �quipe
        if (copsN.Value < copsLimit.Value && !alreadyCop && !alreadyRunner)
        {
            copsPlayerNameTxt[copsN.Value] = LM.playerName;
            copsPlayerNameTMPro[copsN.Value].text = copsPlayerNameTxt[copsN.Value];

            PlayerNameCops(LM.playerName);

            MorecopsNValueServerRpc();
            copsNumberTxt.text = copsN.Value.ToString();

            alreadyCop = true;
        }

        // Si je suis d�j� dans l'�quipe des courreurs et qu'il y a de la place chez les policiers
        if (alreadyRunner && copsN.Value < copsLimit.Value && !alreadyCop)
        {
            alreadyRunner = false;
            alreadyCop = true;

            // On ajoute notre nom � la liste des policiers
            copsPlayerNameTxt[copsN.Value] = LM.playerName;
            copsPlayerNameTMPro[copsN.Value].text = copsPlayerNameTxt[copsN.Value];

            PlayerNameCops(LM.playerName);

            MorecopsNValueServerRpc();

            // On retire de la liste des courreurs notre nom
            int index = runnersPlayerNameTxt.IndexOf(LM.playerName);
            runnersPlayerNameTxt[index] = "";
            runnersPlayerNameTMPro[index].text = "";

            RemovePlayerNameRunners(LM.playerName);

            LessrunnersNValueServerRpc();

            // Nombre de policier et de courreur
            runnersNumberTxt.text = runnersN.Value.ToString();
            copsNumberTxt.text = copsN.Value.ToString();
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

        // S'il y a de la place et que je ne suis pas encore dans cette �quipe
        if (runnersN.Value < runnersLimit.Value && !alreadyRunner && !alreadyCop)
        {
            runnersPlayerNameTxt[runnersN.Value] = LM.playerName;
            runnersPlayerNameTMPro[runnersN.Value].text = runnersPlayerNameTxt[runnersN.Value];

            PlayerNameRunners(LM.playerName);

            MorerunnersNValueServerRpc();
            runnersNumberTxt.text = runnersN.Value.ToString();

            alreadyRunner = true;
        }

        // Si je suis d�j� dans l'�quipe des policiers et qu'il y a de la place chez les courreurs
        if (alreadyCop && runnersN.Value < runnersLimit.Value && !alreadyRunner)
        {
            alreadyCop = false;
            alreadyRunner = true;

            // On ajoute notre nom � la liste des courreurs
            runnersPlayerNameTxt[runnersN.Value] = LM.playerName;
            runnersPlayerNameTMPro[runnersN.Value].text = runnersPlayerNameTxt[runnersN.Value];

            PlayerNameRunners(LM.playerName);

            MorerunnersNValueServerRpc();

            // On retire de la liste des policiers notre nom
            int index = copsPlayerNameTxt.IndexOf(LM.playerName);
            copsPlayerNameTxt[index] = "";
            copsPlayerNameTMPro[index].text = "";

            LesscopsNValueServerRpc();

            RemovePlayerNameCops(LM.playerName);

            // Nombre de policier et de courreur
            copsNumberTxt.text = copsN.Value.ToString();
            runnersNumberTxt.text = runnersN.Value.ToString();
        }
    }

    public void Equilibrage()
    {
        switch (LM.maxPlayers)
        {
            case 8:
                Debug.Log("Il y a 2 Cops | 6 Runners");
                copsLimitValueServerRpc(2);
                runnersLimitValueServerRpc(6);
                break;
            case 7:
                Debug.Log("Il y a 2 Cops | 5 Runners");
                copsLimitValueServerRpc(2);
                runnersLimitValueServerRpc(5);
                break;
            case 6:
                Debug.Log("Il y a 2 Cops | 4 Runners");
                copsLimitValueServerRpc(2);
                runnersLimitValueServerRpc(4);
                break;
            case 5:
                Debug.Log("Il y a 1 Cops | 4 Runners");
                copsLimitValueServerRpc(1);
                runnersLimitValueServerRpc(4);
                break;
            case 4:
                Debug.Log("Il y a 1 Cops | 3 Runners");
                copsLimitValueServerRpc(1);
                runnersLimitValueServerRpc(3);
                break;
            case 2:
                Debug.Log("Test 1 joueur de chaque");
                copsLimitValueServerRpc(1);
                runnersLimitValueServerRpc(1);
                break;
        }

        copsMaxNumberTxt.text = copsLimit.Value.ToString();
        runnersMaxNumberTxt.text = runnersLimit.Value.ToString();

        for (int i = 0; i < copsLimit.Value; i++)
        {
            copsFondText[i].SetActive(true);
        }

        for (int i = 0; i < runnersLimit.Value; i++)
        {
            runnersFondText[i].SetActive(true);
        }

        showTimer = true;
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
                    RemovePlayerNameRunners(LM.playerName);
                    LessrunnersNValueServerRpc();
                    runnersNumberTxt.text = runnersN.Value.ToString();
                }
            }

            alreadyRunner = false;
            runnersPlayerNameTxt[runnersN.Value] = "";
            runnersPlayerNameTMPro[runnersN.Value].text = "";

            for (int i = 0; i < copsPlayerNameTxt.Count; i++)
            {
                if (copsPlayerNameTxt[i].Contains(LM.playerName))
                {
                    RemovePlayerNameCops(LM.playerName);
                    LesscopsNValueServerRpc();
                    copsNumberTxt.text = copsN.Value.ToString();
                }
            }

            alreadyCop = false;
            copsPlayerNameTxt[copsN.Value] = "";
            copsPlayerNameTMPro[copsN.Value].text = "";

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