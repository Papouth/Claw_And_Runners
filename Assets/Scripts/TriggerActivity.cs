using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivity : MonoBehaviour
{
    [SerializeField] private GameObject activityPrefab;

    [SerializeField] private bool stand2TirActivity;
    private StandTir standTir;

    private PlayerActivity activity;
    private GameObject player;
    private bool startActivity;
    private bool interactActivity;


    private void Awake()
    {
        if (stand2TirActivity) standTir = activityPrefab.GetComponent<StandTir>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.GetComponent<InputManager>().CanInteract)
            {
                InteractActivity();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            startActivity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerInfo>().isCops)
            {
                activity = other.GetComponent<PlayerActivity>();
                activity.Pistol(false);

                other.GetComponentInChildren<PlayerShoot>().playerAnimator.SetBool("PistolOn", false);

                startActivity = false;
                interactActivity = false;
            }
        }
    }

    private void InteractActivity()
    {
        if (startActivity && !interactActivity)
        {
            interactActivity = true;

            // Reset activité
            standTir.ResetActivity();

            // Pour l'instant stand de tir dispo seulement pour le policier
            if (player.GetComponent<PlayerInfo>().isCops)
            {
                activity = player.GetComponent<PlayerActivity>();
                activity.Pistol(true);

                player.GetComponentInChildren<PlayerShoot>().playerAnimator.SetBool("PistolOn", true);
            }
        }
        
    }
}