using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager GM;
    [SerializeField] private TeamSelection TS;
    [SerializeField] private float timeRemaining;
    [SerializeField] private TextMeshProUGUI infoTime;
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
            //Perdu();
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Envoyé ça au serveur pour le synchroniser avec les autres clients
        //infoTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion
}