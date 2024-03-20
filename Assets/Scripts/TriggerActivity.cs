using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TriggerActivity : NetworkBehaviour
{
    [SerializeField] private GameObject activityPrefab;

    [SerializeField] private bool stand2TirActivity;
    private StandTir standTir;

    private PlayerActivity activity;
    private GameObject player;
    private bool startActivity;
    private bool interactActivity;
    [SerializeField] private LayerMask playerLayer;



    public override void OnNetworkSpawn()
    {
        if (stand2TirActivity) standTir = activityPrefab.GetComponent<StandTir>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.GetComponent<InputManager>().CanInteract && IsOwner)
            {
                player.GetComponent<InputManager>().CanInteract = false;
                Debug.Log("interact");
                InteractActivity();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Debug.Log(other.name + " here");

            player = other.gameObject;
            startActivity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            if (other.GetComponent<PlayerInfo>().isCops)
            {
                activity = other.GetComponent<PlayerActivity>();

                if (stand2TirActivity)
                {
                    activity.Pistol(false);
                    other.GetComponentInChildren<PlayerShoot>().playerAnimator.SetBool("PistolOn", false);
                }

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

            if (stand2TirActivity)
            {
                Debug.Log("J'interragis avec le stand de tir");

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
        else if (startActivity && interactActivity)
        {
            // Si on ré-interragi, alors on quitte l'activité
            player.GetComponentInChildren<PlayerShoot>().playerAnimator.SetBool("PistolOn", false);

            startActivity = false;
            interactActivity = false;
        }
    }
}