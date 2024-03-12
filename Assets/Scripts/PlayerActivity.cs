using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActivity : MonoBehaviour
{
    [Header("Stand2Tir")]
    [SerializeField] private GameObject pistolPrefab;


    public void Pistol(bool state)
    {
        if (state)
        {
            pistolPrefab.SetActive(true);
        }
        else if (!state)
        {
            pistolPrefab.SetActive(false);
        }
    }
}