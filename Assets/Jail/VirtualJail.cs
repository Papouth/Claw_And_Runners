using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VirtualJail : MonoBehaviour
{
    #region Vairables
    [SerializeField] private GameObject debugSphere;
    [SerializeField] private GameObject jailParent;
    [SerializeField] private int numDebugSpheres;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float spheresRadius;
    private GameObject cloneJail;
    private GameObject cloneSphere;
    [SerializeField] private List<GameObject> spheresList = new List<GameObject>();
    [SerializeField] private LayerMask layerWall;
    private bool prisonOn;
    private LineRenderer lineRenderer;
    private InputManager inputManager;
    #endregion


    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.CanSelect && !prisonOn)
        {
            CreateDebugJail(numDebugSpheres, new Vector3(transform.position.x, transform.position.y + (spheresRadius * 2f), transform.position.z), distanceFromPlayer);
            prisonOn = true;
        }
    }

    private void CreateDebugJail(int num, Vector3 point, float radius)
    {
        cloneJail = Instantiate(jailParent, transform.position, Quaternion.identity);

        lineRenderer = cloneJail.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = numDebugSpheres;

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

            //cloneSphere.name = i.ToString();

            spheresList.Add(cloneSphere);

            cloneSphere.transform.localScale = new Vector3(spheresRadius, spheresRadius, spheresRadius);

            cloneSphere.transform.LookAt(point);
        }

        Invoke("SetLineRenderer", 2f);
    }

    private void SetLineRenderer()
    {
        lineRenderer.enabled = true;

        for (int a = 0; a < spheresList.Count; a++)
        {
            lineRenderer.SetPosition(a, spheresList[a].transform.position);
            Destroy(spheresList[a].GetComponent<SphereCollider>());

            var bCol = spheresList[a].AddComponent<BoxCollider>();
            bCol.transform.localScale = new Vector3(0.08f, 30, 0.08f);

            bCol.transform.gameObject.layer = 13;
        }

        // Opti
        lineRenderer.Simplify(0.01f);

        CheckSurface();
    }

    private void CheckSurface()
    {
        var numS = (spheresList.Count / 4);
        Debug.Log(spheresList.Count);
        Debug.Log(numS);

        float distance = Vector3.Distance(spheresList[0].transform.position, spheresList[numS * 2].transform.position);
        float distance2 = Vector3.Distance(spheresList[numS].transform.position, spheresList[spheresList.Count-1].transform.position);

        if (distance <= 0.8f || distance2 <= 0.8f)
        {
            Debug.Log("mur trop rapproché");

            Destroy(cloneJail);

            prisonOn = true;
        }
    }
}