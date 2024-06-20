using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSphere : MonoBehaviour
{
    #region Variables
    private bool isValid;
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private float rad;
    private float speed;
    #endregion


    #region Built in Methods
    private void Start()
    {
        speed = 4f;
        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        rad = sphereCollider.radius;

        Invoke("StopMotion", 2f/speed);
    }

    private void Update()
    {
        if (!isValid) transform.Translate(-Vector3.forward * Time.deltaTime * speed);

        CheckGround();
    }
    #endregion


    #region Customs Methods
    private void CheckGround()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, rad * 2f) && !isValid)
        {
            //Debug.Log("hors sol");
            StopMotion();
        }
    }

    private void StopMotion()
    {
        isValid = true;
        rb.isKinematic = true;
        sphereCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3 && !isValid)
        {
            StopMotion();
        }
    }
    #endregion
}