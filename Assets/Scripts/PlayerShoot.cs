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
    #endregion


    #region Built-In Methods
    public override void OnNetworkSpawn()
    {
        inputManager = GetComponentInParent<InputManager>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        Shoot();
    }
    #endregion


    #region Customs Methods
    private void Shoot()
    {
        if (inputManager.CanSelect && IsOwner)
        {
            InstantiateBullet();
        }
    }

    private void InstantiateBullet()
    {
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