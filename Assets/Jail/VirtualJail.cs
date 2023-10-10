using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VirtualJail : MonoBehaviour
{
    #region Vairables
    [SerializeField] private GameObject debugSphere;
    [SerializeField] private GameObject debugTwo;
    [SerializeField] private GameObject jailParent;
    [SerializeField] private int numDebugSpheres;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float spheresRadius;
    private GameObject cloneJail;
    private GameObject cloneSphere;
    [SerializeField] private float speed;
    [SerializeField] private List<GameObject> spheresList = new List<GameObject>();

    private RaycastHit[] hits;
    [SerializeField] private LayerMask layerWall;
    private bool isValid;
    #endregion


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0)) CreateDebugJail(numDebugSpheres, new Vector3(transform.position.x, transform.position.y + (spheresRadius/2f), transform.position.z), distanceFromPlayer);


        if (Input.GetKeyUp(KeyCode.Mouse1)) isValid = true;


        //if (isValid) ValidateJail();
    }

    private void CreateDebugJail(int num, Vector3 point, float radius)
    {
        if (cloneJail != null) Destroy(cloneJail);

        cloneJail = Instantiate(jailParent, transform.position, Quaternion.identity);

        for (int i = 0; i < num; i++)
        {
            // Distance autour du cercle
            var radians = 2 * Mathf.PI / num * i;

            // Direction du vecteur
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            // Position de spawn
            var spawnPos = point + spawnDir * radius;

            // On spawn les spheres
            cloneSphere = Instantiate(debugSphere, spawnPos, Quaternion.identity, cloneJail.transform);

            cloneSphere.name = i.ToString();

            spheresList.Add(cloneSphere);

            cloneSphere.transform.localScale = new Vector3(spheresRadius, spheresRadius, spheresRadius);

            cloneSphere.transform.LookAt(point);
        }

        Invoke("Unvalid", 2f);
    }

    private void ValidateJail()
    {
        Invoke("Unvalid", 2f);


        for (int i = 0; i < spheresList.Count; i++)
        {
            spheresList[i].transform.Translate(-Vector3.forward * Time.deltaTime);
        }
    }

    private void Unvalid()
    {
        if (isValid)
        {
            isValid = false;

            hits = Physics.SphereCastAll(transform.position, 2f, transform.position, 0f, layerWall);

            foreach (RaycastHit hit in hits)
            {
                Debug.Log("hit");

                Instantiate(debugTwo, hit.collider.bounds.ClosestPoint(transform.position), Quaternion.FromToRotation(hit.collider.bounds.ClosestPoint(transform.forward), hit.normal), cloneJail.transform);
            }
        }
    }
}