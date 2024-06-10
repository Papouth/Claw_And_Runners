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

    // Porte
    [SerializeField] private bool porteOption;
    [SerializeField] private float timerCloseDoor;
    private Animator porteAnimator;
    [SerializeField] private bool thisDoorOnly;

    private StandTir standTir;
    private Piano piano;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        // Assignation de l'activité trigger
        if (stand2TirActivity) standTir = activityPrefab.GetComponent<StandTir>();

        if (pianoActivity) piano = activityPrefab.GetComponent<Piano>();

        if (porteOption) porteAnimator = activityPrefab.GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerActivity != null) DoorScript();
    }

    /// <summary>
    /// Booléen en true de l'activité correspondante
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerActivity>())
        {
            if (porteOption)
            {
                playerActivity = other.GetComponent<PlayerActivity>();

                playerActivity.porte = true;

                playerActivity.inTrigger = true;

                thisDoorOnly = true;

                return;
            }

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
            if (porteOption)
            {
                playerActivity.porte = false;

                playerActivity.inTrigger = false;

                thisDoorOnly = false;

                return;
            }

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

    #region Customs Methods
    private void DoorScript()
    {
        if (porteOption && playerActivity.porte && thisDoorOnly)
        {
            // On ouvre la porte
            playerActivity.doorIsOpen = true;

            // Animation ouverture de la porte
            if (porteAnimator != null) porteAnimator.SetBool("OpenDoor", true);

            OpenDoorServerRpc();

            // Déclenchement du timer avant la fermeture
            Invoke("CloseDoor", timerCloseDoor);
        }
    }

    private void CloseDoor()
    {
        CloseDoorServerRpc();

        // On ferme la porte
        playerActivity.doorIsOpen = false;

        // Animation fermeture de la porte
        if (porteAnimator != null) porteAnimator.SetBool("OpenDoor", false);
    }
    #endregion


    #region ServerRpc
    [ServerRpc]
    public void OpenDoorServerRpc()
    {
        OpenDoorClientRpc();

        // On ouvre la porte
        playerActivity.doorIsOpen = true;

        // Animation ouverture de la porte
        if (porteAnimator != null) porteAnimator.SetBool("OpenDoor", true);

        // Déclenchement du timer avant la fermeture
        Invoke("CloseDoor", timerCloseDoor);
    }

    [ServerRpc]
    public void CloseDoorServerRpc()
    {
        CloseDoorClientRpc();

        // On ferme la porte
        playerActivity.doorIsOpen = false;

        // Animation fermeture de la porte
        if (porteAnimator != null) porteAnimator.SetBool("OpenDoor", false);
    }
    #endregion


    #region ClientRpc
    [ClientRpc]
    private void OpenDoorClientRpc()
    {
        // On ouvre la porte
        playerActivity.doorIsOpen = true;

        // Animation ouverture de la porte
        if (porteAnimator != null) porteAnimator.SetBool("OpenDoor", true);

        // Déclenchement du timer avant la fermeture
        Invoke("CloseDoor", timerCloseDoor);
    }


    [ClientRpc]
    private void CloseDoorClientRpc()
    {
        // On ferme la porte
        playerActivity.doorIsOpen = false;

        // Animation fermeture de la porte
        if (porteAnimator != null) porteAnimator.SetBool("OpenDoor", false);
    }
    #endregion
}