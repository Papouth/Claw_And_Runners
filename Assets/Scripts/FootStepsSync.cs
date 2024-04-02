using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FootStepsSync : AudioSync
{
    public override void OnNetworkSpawn()
    {
        //listener = playerCam.GetComponent<AudioListener>();
        source = GetComponent<AudioSource>();

        if (IsLocalPlayer) listener.enabled = true;
        else listener.enabled = false;
    }

    /// <summary>
    /// Sert à jouer un son
    /// </summary>
    /// <param name="id"></param>
    public void PlaySoundStep(int id)
    {
        if (id >= 0 && id < gameSounds.Length)
        {
            SoundIDStepServerRpc(id);
        }
    }

    /// <summary>
    /// Sert à envoyer au serveur l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ServerRpc(RequireOwnership = false)]
    public void SoundIDStepServerRpc(int id)
    {
        SoundIDClientRpc(id);
    }

    /// <summary>
    /// Sert à envoyer au client l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ClientRpc]
    public void SoundIDStepClientRpc(int id)
    {
        source.PlayOneShot(gameSounds[id]);
    }
}