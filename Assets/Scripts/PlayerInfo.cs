using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;


public class PlayerInfo : NetworkBehaviour
{
    #region Variables
    private TeamSelection TS;
    private LobbyManager LM;
    private bool equilibrageOn;
    private int prevCops;
    private int prevRunners;
    [Tooltip("true = chasseur | false = runner")] public bool isCops;
    public int isCopsInt;
    public string playerName;
    [HideInInspector] public int playerId;
    public bool tsReadySelection;
    public bool haveJail;
    private VirtualJail VJ;
    private WeaponCop WC;
    public bool playerCop;
    public GameObject captureCol;

    private FixedString32Bytes copWithJail;
    private bool status;
    private bool playerInJail;
    private GameObject zonz;

    private WeaponRunner WR;
    public GameObject releaseCol;

    private Transform spawnCops;
    private Transform spawnRunners;
    private GameObject[] spawnsRunners;

    private PlayerController controller;

    // Skin Attribution
    //private int randSkinNumber;
    public NetworkVariable<int> randSkinNumber;
    public NetworkVariable<int> runnersCounter;

    public GameObject playerCopPrefab;
    public GameObject[] skinsMauricePrefabs;

    public GameObject playerRunnerPrefab;
    public GameObject[] skinsMichellePrefabs;
    public GameObject[] skinsMichaelPrefabs;
    public GameObject[] skinsMarcelPrefabs;

    private CharacterController CCPlayer;
    private PlayerInventory playerInventory;
    private NetworkParameter NP;
    #endregion


    #region Built-In Methods
    private void Start()
    {
        prevCops = 0;
        prevRunners = 0;
    }

    public override void OnNetworkSpawn()
    {
        randSkinNumber = new NetworkVariable<int>();

        randSkinNumber.OnValueChanged += OnrandSkinNumberChanged;

        randSkinNumber.Value = Random.Range(0, 3);

        TS = FindObjectOfType<TeamSelection>();

        LM = FindObjectOfType<LobbyManager>();

        VJ = GetComponent<VirtualJail>();

        WC = GetComponent<WeaponCop>();
        WR = GetComponent<WeaponRunner>();

        captureCol = GetComponentInChildren<CapturePlayer>().gameObject;
        releaseCol = GetComponentInChildren<ReleasePlayer>().gameObject;

        controller = GetComponent<PlayerController>();

        CCPlayer = GetComponent<CharacterController>();

        playerInventory = GetComponent<PlayerInventory>();

        NP = NetworkManager.GetComponent<NetworkParameter>();
    }

    private void Update()
    {
        // Sert à connaitre l'idéntité de chaque joueur
        if (Input.GetMouseButtonDown(0) && IsOwner && TS.selectionStarted)
        {
            SendClientIDFunction();
        }

        if (IsOwner && !equilibrageOn)
        {
            equilibrageOn = true;

            TS.Equilibrage();

            if (LM == null) LM = FindObjectOfType<LobbyManager>();

            if (IsServer) TS.parcName.text = LM.joinedLobby.Name;
        }

        if (IsOwner)
        {
            if (TS.copsN.Value != prevCops)
            {
                TS.UpdateCopsNValue();

                TS.UpdateSelectionNames();

                prevCops = TS.copsN.Value;
            }

            if (TS.runnersN.Value != prevRunners)
            {
                TS.UpdateRunnersNValue();

                TS.UpdateSelectionNames();

                prevRunners = TS.runnersN.Value;
            }

            if (TS.requireNameUpdate)
            {
                TS.requireNameUpdate = false;

                TS.UpdateSelectionNames();
            }
        }

        SetPlayerInJail();

        ReleasePlayerFromJail();

        if (TS.readySelection)
        {
            tsReadySelection = true;
        }

        if (TS.tagSetup && !status)
        {
            status = true;
            Invoke("StatusPlayer", 1f);
        }
    }
    #endregion


    #region Customs Methods
    private void OnrandSkinNumberChanged(int previous, int current)
    {
        //Debug.Log("Changement de variable copsLimit, précédente valeur : " + previous + " |  nouvelle valeur : " + current);
    }

    private void SetPlayerInJail()
    {
        if (gameObject.layer == 10 && !playerInJail)
        {
            playerInJail = true;

            // On Désactive arme du joueur
            WR.enabled = false;

            JailLayerServerRpc(10);

            CCPlayer.enabled = false;

            if (zonz == null)
            {
                zonz = GameObject.FindWithTag("JailObject");

                gameObject.transform.position = zonz.transform.position;
            }
            else gameObject.transform.position = zonz.transform.position;

            CCPlayer.enabled = true;

            SubmitPositionServerRpc(zonz.transform.position);
        }
    }

    private void ReleasePlayerFromJail()
    {
        if (gameObject.layer == 6 && playerInJail)
        {
            playerInJail = false;

            // On Réactive arme du joueur
            WR.enabled = true;

            JailLayerServerRpc(6);
        }
    }

    /// <summary>
    /// Gère les tags et les scripts du joueur
    /// </summary>
    private void StatusPlayer()
    {
        if (IsOwner)
        {
            if (TS.copsNamesList.Contains(playerName))
            {
                // Attribution du tag et des scripts
                // Augmentation de la vitesse du policier
                controller.speed += 1.35f;

                if (TS.copsNamesList[0] == playerName)
                {
                    // Attribution de la prison au premier policier présent dans la partie
                    copWithJail = TS.copsNamesList[0];
                }

                isCops = true;
                isCopsInt = 1;

                gameObject.tag = "cops";

                playerCopPrefab = skinsMauricePrefabs[randSkinNumber.Value];

                InfoServerRpc(true, 1);

                // On gère l'arme du joueur selon son rôle
                WR.enabled = false;

                releaseCol.SetActive(false);

                WC.enabled = true;
                captureCol.SetActive(false);


                for (int i = 0; i < skinsMauricePrefabs.Length; i++)
                {
                    skinsMauricePrefabs[i].SetActive(false);
                }

                //playerCopPrefab = skinsMauricePrefabs[randSkinNumber];
                playerCopPrefab.SetActive(true);

                for (int i = 0; i < skinsMichellePrefabs.Length; i++)
                {
                    skinsMichellePrefabs[i].SetActive(false);
                    skinsMichaelPrefabs[i].SetActive(false);
                    skinsMarcelPrefabs[i].SetActive(false);
                }

                for (int i = 0; i < playerCopPrefab.transform.childCount; i++)
                {
                    if (playerCopPrefab.transform.GetChild(i).GetComponent<DoNotDisplayThisToPlayer>()) playerCopPrefab.transform.GetChild(i).gameObject.SetActive(false);
                }

                RoleReleaseServerRpc(true);

                // S'il s'agit du policier ayant la prison
                if (playerName == copWithJail)
                {
                    // On enable le script de la prison sur le joueur
                    VJ.enabled = true;

                    RoleJailServerRpc(true);
                }
                else if (playerName != copWithJail)
                {
                    VJ.enabled = false;

                    RoleJailServerRpc(false);
                }

                // Spawn du joueur à la position SpawnCops
                CCPlayer.enabled = false;

                spawnCops = GameObject.FindWithTag("SpawnCops").transform;
                float rand = Random.Range(0f, 2f);
                gameObject.transform.position = new Vector3(spawnCops.position.x + rand, spawnCops.position.y, spawnCops.position.z + rand);
                SpawnPlayerServerRpc(new Vector3(spawnCops.position.x + rand, spawnCops.position.y, spawnCops.position.z + rand));

                CCPlayer.enabled = true;

                // Indication bool PlayerInventory
                playerInventory.skinChoosed = true;
            }
            else if (TS.runnersNamesList.Contains(playerName))
            {
                // Attribution du tag et des scripts
                isCops = false;
                isCopsInt = 2;

                gameObject.tag = "runners";

                InfoServerRpc(false, 2);

                VJ.enabled = false;

                // On gère l'arme du joueur selon son rôle
                WC.enabled = false;


                for (int i = 0; i < skinsMichellePrefabs.Length; i++)
                {
                    skinsMichellePrefabs[i].SetActive(false);
                    skinsMichaelPrefabs[i].SetActive(false);
                    skinsMarcelPrefabs[i].SetActive(false);

                    skinsMauricePrefabs[i].SetActive(false);
                }

                if (/*NP.runnersCounter == 0*/runnersCounter.Value == 0)
                {
                    // Michelle Skin
                    playerRunnerPrefab = skinsMichellePrefabs[randSkinNumber.Value];
                }
                else if (/*NP.runnersCounter == 1*/runnersCounter.Value == 1)
                {
                    // Michael Skin
                    playerRunnerPrefab = skinsMichaelPrefabs[randSkinNumber.Value];
                }
                else if (/*NP.runnersCounter == 2*/runnersCounter.Value == 2)
                {
                    // Marcel Skin
                    playerRunnerPrefab = skinsMarcelPrefabs[randSkinNumber.Value];
                }
                playerRunnerPrefab.SetActive(true);
                //NP.runnersCounter++;
                runnersCounter.Value++;

                for (int i = 0; i < playerRunnerPrefab.transform.childCount; i++)
                {
                    if (playerRunnerPrefab.transform.GetChild(i).GetComponent<DoNotDisplayThisToPlayer>()) playerRunnerPrefab.transform.GetChild(i).gameObject.SetActive(false);
                }

                captureCol.SetActive(false);

                WR.enabled = true;
                releaseCol.SetActive(false);

                RoleJailServerRpc(false);
                RoleCaptureServerRpc(false);

                // Spawn du joueur à la position SpawnRunners
                CCPlayer.enabled = false;

                spawnsRunners = GameObject.FindGameObjectsWithTag("SpawnRunners");
                spawnRunners = spawnsRunners[Random.Range(0, spawnsRunners.Length)].transform;


                float rand = Random.Range(0f, 2f);
                gameObject.transform.position = new Vector3(spawnRunners.position.x + rand, spawnRunners.position.y, spawnRunners.position.z + rand);
                SpawnPlayerServerRpc(new Vector3(spawnRunners.position.x + rand, spawnRunners.position.y, spawnRunners.position.z + rand));

                CCPlayer.enabled = true;

                // Indication bool PlayerInventory
                playerInventory.skinChoosed = true;
            }
        }
    }
    #endregion


    #region ServerRpc
    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(Vector3 spawnPos)
    {
        gameObject.transform.position = spawnPos;

        SpawnPlayerClientRpc(spawnPos);
    }

    [ServerRpc(RequireOwnership = false)]
    private void JailLayerServerRpc(int layer)
    {
        gameObject.layer = layer;

        if (layer == 10)
        {
            // Le joueur est en prison, on lui enlève le script de son arme
            WR.enabled = false;
        }
        else if (layer == 6)
        {
            // Le joueur est libéré, on lui remet le script de son arme
            WR.enabled = true;
        }

        JailLayerClientRpc(layer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitPositionServerRpc(Vector3 pos)
    {
        gameObject.transform.position = pos;

        SubmitPositionClientRpc(pos);
    }

    [ServerRpc]
    public void InfoServerRpc(bool playerIsCops, int playerIsCopsInt)
    {
        isCopsInt = playerIsCopsInt;
        isCops = playerIsCops;


        if (playerIsCops)
        {
            gameObject.tag = "cops";

            playerCopPrefab = skinsMauricePrefabs[randSkinNumber.Value];

            for (int i = 0; i < skinsMauricePrefabs.Length; i++)
            {
                skinsMauricePrefabs[i].SetActive(false);
            }

            playerCopPrefab.SetActive(true);

            for (int i = 0; i < skinsMichellePrefabs.Length; i++)
            {
                skinsMichellePrefabs[i].SetActive(false);
                skinsMichaelPrefabs[i].SetActive(false);
                skinsMarcelPrefabs[i].SetActive(false);
            }
        }
        else if (!playerIsCops)
        {
            gameObject.tag = "runners";

            for (int i = 0; i < skinsMichellePrefabs.Length; i++)
            {
                skinsMichellePrefabs[i].SetActive(false);
                skinsMichaelPrefabs[i].SetActive(false);
                skinsMarcelPrefabs[i].SetActive(false);

                skinsMauricePrefabs[i].SetActive(false);
            }

            if (/*NP.runnersCounter == 0*/runnersCounter.Value == 0)
            {
                // Michelle Skin
                playerRunnerPrefab = skinsMichellePrefabs[randSkinNumber.Value];
            }
            else if (/*NP.runnersCounter == 1*/runnersCounter.Value == 1)
            {
                // Michael Skin
                playerRunnerPrefab = skinsMichaelPrefabs[randSkinNumber.Value];
            }
            else if (/*NP.runnersCounter == 2*/runnersCounter.Value == 2)
            {
                // Marcel Skin
                playerRunnerPrefab = skinsMarcelPrefabs[randSkinNumber.Value];
            }
            playerRunnerPrefab.SetActive(true);
        }

        UpdateServerInfoClientRpc(isCops, isCopsInt);
    }

    [ServerRpc]
    public void RoleJailServerRpc(bool playerJail)
    {
        haveJail = playerJail;

        if (!haveJail && VJ != null) VJ.enabled = false;
        else if (haveJail && !VJ.enabled) VJ.enabled = true;

        UpdateServerRoleJailClientRpc(haveJail);
    }

    [ServerRpc]
    public void RoleCaptureServerRpc(bool playerCoporNot)
    {
        playerCop = playerCoporNot;

        if (!playerCop && WC != null)
        {
            WC.enabled = false;
        }
        else if (playerCop && !WC.enabled)
        {
            WC.enabled = true;
        }

        if (!playerCop && captureCol != null)
        {
            captureCol.SetActive(false);
        }

        UpdateServerRoleCaptureClientRpc(playerCop);
    }

    [ServerRpc]
    public void RoleReleaseServerRpc(bool playerCoporNot)
    {
        playerCop = playerCoporNot;

        if (playerCop && WR != null)
        {
            WR.enabled = false;
        }
        else if (!playerCop && !WR.enabled)
        {
            WR.enabled = true;
        }

        if (playerCop && captureCol != null) releaseCol.SetActive(false);

        UpdateServerRoleReleaseClientRpc(playerCop);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendClientIDServerRpc(ulong clientId)
    {
        playerId = (int)clientId;
        NetworkParameter.lastIdSave = playerId;
    }
    #endregion

    public void SendClientIDFunction()
    {
        SendClientIDServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    #region ClientRpc
    [ClientRpc]
    private void SpawnPlayerClientRpc(Vector3 spawnPos)
    {
        gameObject.transform.position = spawnPos;
    }

    [ClientRpc]
    private void JailLayerClientRpc(int layer)
    {
        gameObject.layer = layer;

        if (layer == 10)
        {
            // Le joueur est en prison, on lui enlève le script de son arme
            WR.enabled = false;
        }
        else if (layer == 6)
        {
            // Le joueur est libéré, on lui remet le script de son arme
            WR.enabled = true;
        }
    }

    [ClientRpc]
    private void SubmitPositionClientRpc(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    [ClientRpc]
    public void UpdateServerPlayerNameClientRpc(string newplayername)
    {
        playerName = newplayername;
        gameObject.name = playerName;
    }

    [ClientRpc]
    public void UpdateServerInfoClientRpc(bool playerIsCops, int playerIsCopsInt)
    {
        isCopsInt = playerIsCopsInt;
        isCops = playerIsCops;

        if (playerIsCops)
        {
            gameObject.tag = "cops";

            playerCopPrefab = skinsMauricePrefabs[randSkinNumber.Value];

            for (int i = 0; i < skinsMauricePrefabs.Length; i++)
            {
                skinsMauricePrefabs[i].SetActive(false);
            }

            playerCopPrefab.SetActive(true);

            for (int i = 0; i < skinsMichellePrefabs.Length; i++)
            {
                skinsMichellePrefabs[i].SetActive(false);
                skinsMichaelPrefabs[i].SetActive(false);
                skinsMarcelPrefabs[i].SetActive(false);
            }
        }
        else if (!playerIsCops)
        {
            gameObject.tag = "runners";

            for (int i = 0; i < skinsMichellePrefabs.Length; i++)
            {
                skinsMichellePrefabs[i].SetActive(false);
                skinsMichaelPrefabs[i].SetActive(false);
                skinsMarcelPrefabs[i].SetActive(false);

                skinsMauricePrefabs[i].SetActive(false);
            }

            if (/*NP.runnersCounter == 0*/runnersCounter.Value == 0)
            {
                // Michelle Skin
                playerRunnerPrefab = skinsMichellePrefabs[randSkinNumber.Value];
            }
            else if (/*NP.runnersCounter == 1*/runnersCounter.Value == 1)
            {
                // Michael Skin
                playerRunnerPrefab = skinsMichaelPrefabs[randSkinNumber.Value];
            }
            else if (/*NP.runnersCounter == 2*/runnersCounter.Value == 2)
            {
                // Marcel Skin
                playerRunnerPrefab = skinsMarcelPrefabs[randSkinNumber.Value];
            }
            playerRunnerPrefab.SetActive(true);
        }
    }

    [ClientRpc]
    public void UpdateServerRoleJailClientRpc(bool playerJail)
    {
        haveJail = playerJail;

        if (!haveJail && VJ != null) VJ.enabled = false;
        else if (haveJail && !VJ.enabled) VJ.enabled = true;
    }

    [ClientRpc]
    public void UpdateServerRoleCaptureClientRpc(bool playerCoporNot)
    {
        playerCop = playerCoporNot;

        if (!playerCop && WC != null) WC.enabled = false;
        else if (playerCop && !WC.enabled) WC.enabled = true;

        if (!playerCop && captureCol != null) captureCol.SetActive(false);
    }

    [ClientRpc]
    public void UpdateServerRoleReleaseClientRpc(bool playerCoporNot)
    {
        playerCop = playerCoporNot;

        if (playerCop && WR != null) WR.enabled = false;
        else if (!playerCop && !WR.enabled) WR.enabled = true;

        if (playerCop && captureCol != null) releaseCol.SetActive(false);
    }
    #endregion
}