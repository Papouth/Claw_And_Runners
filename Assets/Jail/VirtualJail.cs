using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Netcode;

public class VirtualJail : NetworkBehaviour
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
    private BoxCollider bCol;
    private PlayerInfo PI;
    #endregion


    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        PI = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        // Déclenchement de la pose de la prison pour le flic | Rajout check en main du poseur de prison + après que le joueur soit TP à sa position initiale de jeu
        if (IsOwner && inputManager.CanSelect && !prisonOn && PI.tsReadySelection)
        {
            Debug.Log("passe michel");

            CreateDebugJail(numDebugSpheres, new Vector3(transform.position.x, transform.position.y + (spheresRadius * 2f), transform.position.z), distanceFromPlayer);
            prisonOn = true;
        }

        if (inputManager.CanSelect)
        {
            inputManager.CanSelect = false;
        }
    }

    private void CreateDebugJail(int num, Vector3 point, float radius)
    {
        cloneJail = Instantiate(jailParent, transform.position, Quaternion.identity);

        lineRenderer = cloneJail.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = numDebugSpheres;

        cloneJail.GetComponent<NetworkObject>().Spawn();

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

            spheresList.Add(cloneSphere);

            cloneSphere.transform.localScale = new Vector3(spheresRadius, spheresRadius, spheresRadius);

            cloneSphere.transform.LookAt(point);
        }

        Invoke("SetLineRenderer", 0.5f);
    }

    private void SetLineRenderer()
    {
        lineRenderer.enabled = true;

        for (int a = 0; a < spheresList.Count; a++)
        {
            lineRenderer.SetPosition(a, spheresList[a].transform.position);
            Destroy(spheresList[a].GetComponent<SphereCollider>());

            bCol = spheresList[a].AddComponent<BoxCollider>();
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

        float distance = Vector3.Distance(spheresList[0].transform.position, spheresList[numS * 2].transform.position);
        float distance2 = Vector3.Distance(spheresList[numS].transform.position, spheresList[spheresList.Count - 1].transform.position);

        if (distance <= 0.8f || distance2 <= 0.8f)
        {
            Debug.Log("mur trop rapproché");

            spheresList.Clear();
            Destroy(cloneJail);

            // Possibilité de reposer une prison si la précédente n'est pas valide
            prisonOn = false;
        }
        else
        {
            // On rebouche les trous
            for (int i = 0; i < spheresList.Count; i++)
            {
                if (i < spheresList.Count - 1)
                {
                    float dist = Vector3.Distance(spheresList[i].transform.position, spheresList[i + 1].transform.position);

                    if (dist >= 0.5f)
                    {
                        // Le Vecteur Forward est dirigé vers la sphère suivante
                        spheresList[i].transform.LookAt(spheresList[i + 1].transform);

                        bCol = spheresList[i].GetComponent<BoxCollider>();
                        bCol.transform.localScale = new Vector3(0.08f, 30f, dist);
                        bCol.center = new Vector3(0f, 0f, dist / 2f);
                    }
                }
                else if (i == spheresList.Count - 1)
                {
                    float dist = Vector3.Distance(spheresList[i].transform.position, spheresList[0].transform.position);

                    if (dist >= 0.5f)
                    {
                        // Le Vecteur Forward est dirigé vers la sphère suivante
                        spheresList[i].transform.LookAt(spheresList[0].transform);

                        bCol = spheresList[i].GetComponent<BoxCollider>();
                        bCol.transform.localScale = new Vector3(0.08f, 30f, dist);
                        bCol.center = new Vector3(0f, 0f, dist / 2f);
                    }
                }
            }
        }


        // Ici après on fera le call du server RPC
    }
}