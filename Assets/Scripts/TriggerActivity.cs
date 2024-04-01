using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TriggerActivity : NetworkBehaviour
{
    #region Variables
    [SerializeField] private GameObject activityPrefab;
    [SerializeField] private bool stand2TirActivity;
    private GameObject player;
    private bool startActivity;
    private bool interactActivity;
    [SerializeField] private LayerMask playerLayer;

    private StandTir standTir;
    private PlayerActivity activity;
    private PlayerInventory playerInventory;
    private InputManager inputManager;
    private PlayerShoot playerShoot;
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
        if (stand2TirActivity) standTir = activityPrefab.GetComponent<StandTir>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (inputManager.CanInteract && IsOwner)
            {
                inputManager.CanInteract = false;
                Debug.Log("interact");
                InteractActivity();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerInfo>())
        {
            player = other.GetComponent<PlayerInfo>().gameObject;

            startActivity = true;

            playerInventory = other.GetComponent<PlayerInventory>();
            inputManager = other.GetComponent<InputManager>();

            if (stand2TirActivity) playerShoot = other.GetComponentInChildren<PlayerShoot>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInfo>())
        {
            if (other.GetComponent<PlayerInfo>().isCops) // On pourra le retirer plus tard quand le voleur pourra aussi faire les activités (pas d'anim atm)
            {
                Debug.Log("Je quitte l'activité");

                activity = other.GetComponent<PlayerActivity>();

                if (stand2TirActivity)
                {
                    activity.Pistol(false);
                    playerShoot.playerAnimator.SetBool("PistolOn", false);
                    playerShoot = null;
                }

                startActivity = false;
                interactActivity = false;
                playerInventory.inActivity = false;
                player = null;
                playerInventory = null;
                inputManager = null;
            }
        }
    }
    #endregion


    #region Customs Methods
    private void InteractActivity()
    {
        if (startActivity && !interactActivity)
        {
            interactActivity = true;
            playerInventory.inActivity = true;

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

                    playerShoot.playerAnimator.SetBool("PistolOn", true);
                    playerShoot = null;
                }
            }
        }
        else if (startActivity && interactActivity)
        {
            Debug.Log("Je quitte le stand de tir");

            // Si on ré-interragi, alors on quitte l'activité
            if (stand2TirActivity)
            {
                activity.Pistol(false);
                playerShoot.playerAnimator.SetBool("PistolOn", false);
                playerShoot = null;
            }

            startActivity = false;
            interactActivity = false;
            playerInventory.inActivity = false;
            player = null;
            playerInventory = null;
            inputManager = null;
        }
    }
    #endregion
}