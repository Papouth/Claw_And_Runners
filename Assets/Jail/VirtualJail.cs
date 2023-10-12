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
        if (inputManager.CanSelect && !prisonOn) CreateDebugJail(numDebugSpheres, new Vector3(transform.position.x, transform.position.y + (spheresRadius*2f), transform.position.z), distanceFromPlayer);
    }

    private void CreateDebugJail(int num, Vector3 point, float radius)
    {
        if (cloneJail != null) Destroy(cloneJail);

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
        }

        prisonOn = true;   
    }
}