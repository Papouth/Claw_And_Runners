using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TriggerActivity : MonoBehaviour
{
    #region Variables
    private PlayerActivity playerActivity;

    [SerializeField] private GameObject activityPrefab;
    [SerializeField] private bool stand2TirActivity;

    private StandTir standTir;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        // Assignation de l'activité trigger
        if (stand2TirActivity) standTir = activityPrefab.GetComponent<StandTir>();
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
        }
    }
    #endregion
}