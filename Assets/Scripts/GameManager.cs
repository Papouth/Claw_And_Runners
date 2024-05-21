using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    #region Variables
    public static GameManager GM;
    public bool cheat;
    [SerializeField] private TeamSelection TS;
    public UIManager UIM;

    [Header("Timer")]
    [SerializeField] private GameObject panelTimer;
    [SerializeField] private float timeRemaining;
    [SerializeField] private TextMeshProUGUI infoTime;
    private bool uiTimer;

    public int actualRunnersCaptured;
    public int totalRunnersCaptured;
    public int totalRunnersReleased;
    public NetworkVariable<int> runnersLimitGM;
    private bool winBool;
    private bool setValueLimit;
    [HideInInspector] public GameObject panelShootingRange;
    #endregion

    #region Built In Methods
    private void Awake()
    {
        if (GM != null)
        {
            GameObject.Destroy(GM);
        }
        else
        {
            GM = this;
        }

        DontDestroyOnLoad(this);

        panelTimer.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        // Initialisation Statistiques
        actualRunnersCaptured = 0;
        totalRunnersCaptured = 0;
        totalRunnersReleased = 0;

        panelShootingRange = UIM.panelCrosshair;
        panelShootingRange.SetActive(false);
    }

    private void Update()
    {
        // On vérifie que le décompte du start de la game est terminé
        if (TS.readySelection)
        {
            if (!setValueLimit && IsOwner)
            {
                setValueLimit = true;

                runnersLimitGM.Value = TS.runnersLimit.Value;
            }

            Timer();
            Debug.Log("Il y a " + actualRunnersCaptured + " de runners capturé sur les " + runnersLimitGM.Value + " runners total");
        }
    }
    #endregion


    #region Customs Methods
    public void Timer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else if (timeRemaining <= 0 && !winBool)
        {
            winBool = true;

            // Victoire / Défaite
            UIM.panelEndGame.SetActive(true);

            // RPC Call PanelEndGame
            DisplayPanelEndGameServerRpc();

            Debug.Log("Il y a " + actualRunnersCaptured + " de runners capturé sur les " + TS.runnersLimit.Value + " runners total");

            // Si le policier a capturer tt le monde
            if (actualRunnersCaptured == TS.runnersLimit.Value)
            {
                UIM.winText.text = "Le policier a capturé tout le monde !";

                // RPC Call Cops Win
                DisplayTextCopsWinServerRpc();
            }
            else if (actualRunnersCaptured < TS.runnersLimit.Value)
            {
                // Si les voleurs ne se sont pas tous fait capturer
                UIM.winText.text = "Les voleurs ont remporté la partie !";

                // RPC Call Runners Win
                DisplayTextRunnersWinServerRpc();
            }
        }
    }

    public void CheckCopsWin()
    {
        if (actualRunnersCaptured == runnersLimitGM.Value)
        {
            // Victoire / Défaite
            UIM.panelEndGame.SetActive(true);

            // RPC Call PanelEndGame
            DisplayPanelEndGameServerRpc();

            Debug.Log("Il y a " + actualRunnersCaptured + " de runners capturé sur les " + runnersLimitGM.Value + " runners total");

            UIM.winText.text = "Le policier a capturé tout le monde !";

            // RPC Call Cops Win
            DisplayTextCopsWinServerRpc();
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Envoyé ça au serveur pour le synchroniser avec les autres clients
        infoTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (!uiTimer)
        {
            uiTimer = true;

            panelTimer.SetActive(true);

            DisplayPanelServerRpc();
        }

        DisplayTimeServerRpc(infoTime.text);
    }
    #endregion


    #region ServerRpc
    [ServerRpc(RequireOwnership = false)]
    private void DisplayTimeServerRpc(string time)
    {
        infoTime.text = time;

        DisplayTimeClientRpc(time);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisplayPanelServerRpc()
    {
        if (!uiTimer)
        {
            uiTimer = true;

            panelTimer.SetActive(true);
        }

        DisplayPanelClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisplayPanelEndGameServerRpc()
    {
        UIM.panelEndGame.SetActive(true);

        DisplayPanelEndGameClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisplayTextCopsWinServerRpc()
    {
        UIM.winText.text = "Le policier a capturé tout le monde !";

        DisplayTextCopsWinClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisplayTextRunnersWinServerRpc()
    {
        UIM.winText.text = "Les voleurs ont remporté la partie !";

        DisplayTextRunnersWinClientRpc();
    }

    #endregion


    #region ClientRpc
    [ClientRpc]
    private void DisplayTimeClientRpc(string time)
    {
        infoTime.text = time;
    }

    [ClientRpc]
    private void DisplayPanelClientRpc()
    {
        if (!uiTimer)
        {
            uiTimer = true;

            panelTimer.SetActive(true);
        }
    }

    [ClientRpc]
    private void DisplayPanelEndGameClientRpc()
    {
        UIM.panelEndGame.SetActive(true);
    }

    [ClientRpc]
    private void DisplayTextCopsWinClientRpc()
    {
        UIM.winText.text = "Le policier a capturé tout le monde !";
    }

    [ClientRpc]
    private void DisplayTextRunnersWinClientRpc()
    {
        UIM.winText.text = "Les voleurs ont remporté la partie !";
    }
    #endregion
}