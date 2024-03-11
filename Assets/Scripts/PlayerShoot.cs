using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    #region Variables
    [SerializeField] private float RPM;
    private float rpmMult = 60;
    private float rateOfFire;
    private bool firstBullet;
    private Ray ray;
    [SerializeField] private LayerMask aimColLayermask;

    private Animator playerAnimator;
    private InputManager inputManager;
    #endregion

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        inputManager = GetComponentInParent<InputManager>();

        rateOfFire = rpmMult / RPM;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (inputManager.CanSelect)
        {
            if (!firstBullet)
            {
                firstBullet = true;
                InstantiateBullet();
            }

            rateOfFire -= Time.deltaTime;

            if (rateOfFire < 0)
            {
                rateOfFire = rpmMult / RPM;
                InstantiateBullet();
            }
        }
        else if (inputManager.CanSelect && firstBullet)
        {
            firstBullet = false;
            rateOfFire = rpmMult / RPM;
        }
    }

    private void InstantiateBullet()
    {
        //playerAnimator.Play("PistolShot");

        // Raycast shoot
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 10f, aimColLayermask))
        {
            Debug.Log(raycastHit.collider.name);
            raycastHit.collider.GetComponentInParent<StandTarget>().TargetHit();
        }
    }
}