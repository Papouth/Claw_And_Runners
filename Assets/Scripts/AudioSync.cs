using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class AudioSync : NetworkBehaviour
{
    #region Variables
    public Camera playerCam;
    public AudioListener listener;
    public AudioSource source;
    public AudioClip[] gameSounds;
    #endregion


    #region Built-In Methods
    public virtual void Start()
    {
        source = GetComponent<AudioSource>();

        if (IsLocalPlayer) listener.enabled = true;
        else listener.enabled = false;
    }

    //private void Update()
    //{
    //    VolumeModifier();
    //}
    #endregion


    #region Customs Methods
    /// <summary>
    /// Param�tre rajoutable plus tard
    /// </summary>
    private void VolumeModifier()
    {
        //if (Input.GetKeyDown(KeyCode.B)) AudioListener.volume -= 0.1f;
        //else if (Input.GetKeyDown(KeyCode.N)) AudioListener.volume += 0.1f;
    }

    /// <summary>
    /// Sert � jouer un son
    /// </summary>
    /// <param name="id"></param>
    public void PlaySound(int id)
    {
        if (id >= 0 && id < gameSounds.Length)
        {
            SoundIDServerRpc(id);
        }
    }

    /// <summary>
    /// Sert � envoyer au serveur l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ServerRpc(RequireOwnership = false)]
    public void SoundIDServerRpc(int id)
    {
        SoundIDClientRpc(id);
    }

    /// <summary>
    /// Sert � envoyer au client l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ClientRpc]
    public void SoundIDClientRpc(int id)
    {
        source.PlayOneShot(gameSounds[id]);
    }
    #endregion
}