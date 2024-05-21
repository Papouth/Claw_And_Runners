using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerShoot : NetworkBehaviour
{
    #region Variables
    private Ray ray;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask aimColLayermask;

    public Animator playerAnimator;
    private InputManager inputManager;
    private PlayerActivity playerActivity;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        inputManager = GetComponentInParent<InputManager>();

        playerActivity = GetComponentInParent<PlayerActivity>();
    }

    private void Update()
    {
        if (IsOwner)
        {
            Debug.Log("Passe Update");

            Shoot();
        }
    }
    #endregion


    #region Customs Methods
    /// <summary>
    /// Tir autorisé quand on se trouve dans l'activité de tir
    /// </summary>
    private void Shoot()
    {
        if (playerActivity.playerInActivity) Debug.Log("AAA : Player dans une activité");

        if (playerActivity.standTir) Debug.Log("BBB : Player dans le stand de tir");

        if (inputManager.CanSelect && playerActivity.playerInActivity && playerActivity.standTir)
        {
            inputManager.CanSelect = false;

            Debug.Log("Pre instantiate bullet");

            InstantiateBullet();
        }
    }

    private void InstantiateBullet()
    {
        Debug.Log("Déclenchement animator");

        playerAnimator.SetTrigger("PistolShot");

        Debug.Log("Bang");

        // Raycast shoot
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ray = cam.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 10f, aimColLayermask))
        {
            Debug.Log(raycastHit.collider.name);
            raycastHit.collider.GetComponentInParent<StandTarget>().TargetHit();
        }
    }
    #endregion
}