using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(AudioSync))]
public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private GameObject[] bullet;
    private int bulletTeam;
    private GameObject cloneBullet;
    private PlayerTeam playerHealth;
    private AudioSync audioSync;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerTeam>();
        audioSync = GetComponent<AudioSync>();
    }
    private void Start()
    {
        // Détermine quelle munition va être utilisé
        if (playerHealth.team == "blue") bulletTeam = 0;
        else if (playerHealth.team == "red") bulletTeam = 1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsOwner)
        {
            ShootServerRpc(new Vector3(transform.position.x, 1, transform.position.z + 1), Quaternion.identity);
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 pos, Quaternion rot)
    {
        cloneBullet = Instantiate(bullet[bulletTeam], pos, rot);
        cloneBullet.GetComponent<Rigidbody>().isKinematic = false;

        cloneBullet.GetComponent<NetworkObject>().Spawn();

        PlayShootAudio();
    }
    private void PlayShootAudio()
    {
        audioSync.PlaySound(0);  
    }
}