using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSphere : MonoBehaviour
{
    #region Variables
    private bool isValid;
    private Rigidbody rb;
    private MeshRenderer mr;
    private SphereCollider sphereCollider;
    #endregion


    #region Built in Methods
    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        Invoke("StopMotion", 2f);
    }

    private void Update()
    {
        if (!isValid) transform.Translate(-Vector3.forward * Time.deltaTime);
    }
    #endregion


    #region Customs Methods
    private void StopMotion()
    {
        isValid = true;
        rb.isKinematic = true;
        mr.enabled = false;
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