using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TriggerActivity : MonoBehaviour
{
    #region Variables
    [HideInInspector] public PlayerActivity playerActivity;

    [SerializeField] private GameObject activityPrefab;
    [SerializeField] private bool stand2TirActivity;
    [SerializeField] private bool pianoActivity;

    private StandTir standTir;
    private Piano piano;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        // Assignation de l'activité trigger
        if (stand2TirActivity) standTir = activityPrefab.GetComponent<StandTir>();

        if (pianoActivity) piano = activityPrefab.GetComponent<Piano>();
    }

    /// <summary>
    /// Booléen en true de l'activité correspondante
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerActivity>())
        {
            Debug.Log("Trigger enter activity");

            playerActivity = other.GetComponent<PlayerActivity>();

            playerActivity.inTrigger = true;

            if (stand2TirActivity) playerActivity.standTir = true;

            if (pianoActivity)
            {
                playerActivity.piano = true;

                piano.playerActivity = playerActivity;
                piano.asPlayer = true;
            }
        }
    }

    /// <summary>
    /// Booléen activité correspondante en false + playerActivity = null
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerActivity>())
        {
            Debug.Log("Trigger exit activity");

            playerActivity.inTrigger = false;

            // Reset activité
            if (stand2TirActivity) standTir.ResetActivity();

            if (pianoActivity)
            {
                piano.asPlayer = false;
                piano.playerActivity = null;
            }
        }
    }
    #endregion
}