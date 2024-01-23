using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CapturePlayer : NetworkBehaviour
{
    [SerializeField] private GameObject zonz;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("runners"))
        {
            other.gameObject.layer = 10;
            zonz = GameObject.Find("TheJail");
            // récupérer l'ID du joueur touché
            other.gameObject.transform.position = zonz.transform.position;
        }
    }
}