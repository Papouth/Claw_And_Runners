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

    [Header("Timer")]
    [SerializeField] private GameObject panelTimer;
    [SerializeField] private float timeRemaining;
    [SerializeField] private TextMeshProUGUI infoTime;
    private bool uiTimer;
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

    private void Update()
    {
        // On vérifie que le décompte du start de la game est terminé
        if (TS.readySelection)
        {
            Timer();
        }
    }
    #endregion


    #region Customs Methods
    private void Timer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else if (timeRemaining <= 0)
        {
            // Victoire / Défaite
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
    #endregion
}