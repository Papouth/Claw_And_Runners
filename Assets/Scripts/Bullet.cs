using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float bulletLife;
    [SerializeField] private string team;
    [SerializeField] private float bulletSpeed = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // détruire le gameobjet après X temps

        BulletMove();
    }

    private void BulletMove()
    {
        rb.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (team != other.GetComponent<PlayerHealth>().team)
            {
                Debug.Log("Le joueur est un ennemi !");

                // On inflige les dégâts
                other.GetComponent<PlayerHealth>().playerLife.Value -= damage;
                Debug.Log(other.gameObject.name + " à désormais " + other.GetComponent<PlayerHealth>().playerLife.Value + " HP !");

                Destroy(gameObject);
            }
            if (team == other.GetComponent<PlayerHealth>().team)
            {
                Debug.Log("Attention, tu tir sur tes amis !");

                Destroy(gameObject);
            }
        }
    }
}