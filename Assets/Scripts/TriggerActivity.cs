using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TriggerActivity : MonoBehaviour
{
    #region Variables
    [HideInInspector] public PlayerActivity playerActivity;

    [SerializeField] private GameObject activityPrefab;
    [SerializeField] private bool pianoActivity;

    // Porte
    [SerializeField] private bool porteOption;
    [SerializeField] private float timerCloseDoor;
    private Animator porteAnimator;
    private bool thisDoorOnly;
    [SerializeField] private AudioClip[] doorSounds; // close et open
    private AudioSource audioSource;

    private Piano piano;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        // Assignation de l'activité trigger
        if (pianoActivity) piano = activityPrefab.GetComponent<Piano>();

        if (porteOption) porteAnimator = activityPrefab.GetComponent<Animator>();

        if (porteOption) audioSource = GetComponent<AudioSource>();
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

            //Debug.Log("Trigger enter activity");

            playerActivity = other.GetComponent<PlayerActivity>();

            playerActivity.inTrigger = true;

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

            //Debug.Log("Trigger exit activity");

            playerActivity.inTrigger = false;

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

            PlaySound(1);

            // Déclenchement du timer avant la fermeture
            Invoke("CloseDoor", timerCloseDoor);
        }
    }

    private void CloseDoor()
    {
        CloseDoorServerRpc();

        PlaySound(0);

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


    #region Sounds Management
    /// <summary>
    /// Sert à jouer un son en renseignant un id
    /// </summary>
    /// <param name="id"></param>
    public void PlaySound(int id)
    {
        if (id >= 0 && id < doorSounds.Length)
        {
            SoundIDServerRpc(id);
        }
    }

    /// <summary>
    /// Sert à envoyer au serveur l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ServerRpc(RequireOwnership = false)]
    public void SoundIDServerRpc(int id)
    {
        SoundIDClientRpc(id);
    }

    /// <summary>
    /// Sert à envoyer au client l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ClientRpc]
    public void SoundIDClientRpc(int id)
    {
        audioSource.PlayOneShot(doorSounds[id]);
    }
    #endregion
}