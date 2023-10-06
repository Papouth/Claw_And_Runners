using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


[RequireComponent(typeof(AudioSource))]
public class ObjectAudioSync : NetworkBehaviour
{
    #region Variables
    private AudioSource source;
    [SerializeField] private AudioClip[] gameSounds;
    [SerializeField] private NetworkParameter NP;
    private int baseCounter;
    private float timer;
    private bool startTimer;

    private ClientRpcParams clientRpcParams;
    #endregion


    #region Built-in Methods
    private void Awake()
    {
        timer = 0;
        baseCounter = 0;
        source = GetComponent<AudioSource>();

        if (baseCounter == 0) ObjectPlaySound(0);
    }

    public void Update()
    {
        SoundSynchro();

        TimerAudio();
    }
    #endregion

    #region Customs Methods
    private void TimerAudio()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }
    }

    private void SoundSynchro()
    {
        if (NP.clientCount == 1 && baseCounter == 0)
        {
            baseCounter = 1;

            ObjectPlaySound(0);

            startTimer = true;
        }
        else if (NP.clientCount > 1 && NP.clientCount != baseCounter)
        {
            baseCounter++;

            clientRpcParams = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams()
                {
                    TargetClientIds = new ulong[] {(ulong)baseCounter - 1}
                }
            };
            DelayObjectSoundIDClientRpc(0, timer, clientRpcParams);
        }
    }
    #endregion

    #region Server Interaction
    /// <summary>
    /// Sert à jouer un son
    /// </summary>
    /// <param name="id"></param>
    public void ObjectPlaySound(int id)
    {
        if (id >= 0 && id < gameSounds.Length)
        {
            ObjectSoundIDServerRpc(id);
        }
    }

    /// <summary>
    /// Sert à envoyer au serveur l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ServerRpc(RequireOwnership = false)]
    public void ObjectSoundIDServerRpc(int id)
    {
        ObjectSoundIDClientRpc(id);
    }

    /// <summary>
    /// Sert à envoyer au client l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ClientRpc]
    public void ObjectSoundIDClientRpc(int id)
    {
        source.PlayOneShot(gameSounds[id]);
    }

    [ClientRpc]
    public void DelayObjectSoundIDClientRpc(int id, float time, ClientRpcParams clientRpcParams = default)
    {
        source.clip = gameSounds[id];
        source.time = time;

        source.Play();
    }
    #endregion
}