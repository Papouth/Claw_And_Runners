using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Jail : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject debugSphere;
    private GameObject cloneJail;
    [SerializeField] private GameObject theJail;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private LayerMask layerWall;
    private RaycastHit[] hits;
    [SerializeField] private GameObject edgePrefab;

    [SerializeField] private List<GameObject> debugSpheresList;
    private GameObject cloneDebugSphere;
    #endregion

    private void Update()
    {
        CreatingJail();
    }


    private void CreatingJail()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //if (cloneJail != null) Destroy(cloneJail);
            //
            //cloneJail = Instantiate(theJail, transform.position, Quaternion.identity);
            //
            //Debug.Log("Instantiating a jail");
            //
            //hits = Physics.SphereCastAll(transform.position, 2f, transform.position, 0f, layerWall);
            //
            //foreach (RaycastHit hit in hits)
            //{
            //    Debug.Log("hit");
            //
            //    cloneDebugSphere = Instantiate(debugSphere, hit.collider.bounds.ClosestPoint(transform.position), Quaternion.FromToRotation(hit.collider.bounds.ClosestPoint(transform.forward), hit.normal), cloneJail.transform);
            //    debugSpheresList.Add(cloneDebugSphere);
            //}
            //
            //
            //// Ensuite on relie les debugs sphere entre elles par des murs
        }
    }
}