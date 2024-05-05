using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class Piano : NetworkBehaviour
{
    #region Variables
    [Header("Music Sheets")]
    [SerializeField] private GameObject panelPiano;
    [Tooltip("Sheets music that can be played")]
    [SerializeField] private Sprite[] sheets;
    [SerializeField] private GameObject baseSheet;
    private bool state;
    private int actualSheet;
    [SerializeField] private GameObject personalSheet;
    private bool state2;

    [SerializeField] private float actualVolume;
    [SerializeField] private TextMeshProUGUI volAmount;

    [SerializeField] private AudioClip[] notes;
    [SerializeField] private GameObject[] keys;

    [HideInInspector] public bool asPlayer;

    private AudioSource audioSource;
    [HideInInspector] public PlayerActivity playerActivity;
    #endregion


    #region Built-in Methods
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        personalSheet.SetActive(state2);
        baseSheet.SetActive(state);
        actualSheet = 0;

        baseSheet.GetComponentInChildren<Image>().sprite = sheets[actualSheet];

        actualVolume = 0.5f;

        panelPiano.SetActive(false);
    }

    private void Update()
    {
        if (asPlayer)
        {
            if (playerActivity.playerInActivity && playerActivity.piano)
            {
                PianoInputs();

                SheetDisplay();

                Volume();
            }
        } 
    }
    #endregion

    #region Customs Methods
    /// <summary>
    /// Piano Inputs Management
    /// </summary>
    private void PianoInputs()
    {
        #region Whites and Blacks Keys
        // Key 1 and 1 UP / !
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlaySound(2);

                keys[35].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaySound(1);

            keys[39].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key 2 and 2 UP / @
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlaySound(4);

                keys[36].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaySound(3);

            keys[38].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key 4 and 4 UP / $
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlaySound(7);

                keys[0].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlaySound(6);

            keys[6].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key 5 and 5 UP / %
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PlaySound(9);

                keys[1].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlaySound(8);

            keys[4].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key 6 and 6 UP / ^
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                PlaySound(11);

                keys[2].GetComponent<Animator>().Play("BlackRight");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlaySound(10);

            keys[5].GetComponent<Animator>().Play("WhiteMidRight");
        }


        // Key 8 and 8 UP / *
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                PlaySound(14);

                keys[50].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            PlaySound(13);

            keys[54].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key 9 and 9 UP / (
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                PlaySound(16);

                keys[51].GetComponent<Animator>().Play("BlackRight");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlaySound(15);

            keys[53].GetComponent<Animator>().Play("WhiteMid");
        }


        // Key A and A UP -
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlaySound(45);

                keys[21].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySound(44);

            keys[27].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key Z and Z UP -
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                PlaySound(55);

                keys[22].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            PlaySound(54);

            keys[25].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key E and E UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlaySound(25);

                keys[23].GetComponent<Animator>().Play("BlackRight");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PlaySound(24);

            keys[26].GetComponent<Animator>().Play("WhiteMidRight");
        }


        // Key T and T UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                PlaySound(50);

                keys[45].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            PlaySound(49);

            keys[49].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key Y and Y UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                PlaySound(58);

                keys[46].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            PlaySound(57);

            keys[48].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key I and I UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                PlaySound(32);

                keys[14].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            PlaySound(31);

            keys[20].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key O and O UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                PlaySound(41);

                keys[15].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            PlaySound(40);

            keys[18].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key P and P UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PlaySound(43);

                keys[16].GetComponent<Animator>().Play("BlackRight");
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PlaySound(42);

            keys[19].GetComponent<Animator>().Play("WhiteMidRight");
        }


        // Key S and S UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlaySound(48);

                keys[55].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            PlaySound(47);

            keys[59].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key D and D UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlaySound(23);

                keys[56].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlaySound(22);

            keys[58].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key G and G UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                PlaySound(28);

                keys[28].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            PlaySound(27);

            keys[34].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key H and H UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                PlaySound(30);

                keys[29].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            PlaySound(29);

            keys[32].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key J and J UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                PlaySound(34);

                keys[30].GetComponent<Animator>().Play("BlackRight");
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            PlaySound(33);

            keys[33].GetComponent<Animator>().Play("WhiteMidRight");
        }


        // Key L and L UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                PlaySound(37);

                keys[40].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PlaySound(36);

            keys[44].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key W and W UP -
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                PlaySound(60);

                keys[41].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            PlaySound(59);

            keys[43].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key C and C UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                PlaySound(21);

                keys[7].GetComponent<Animator>().Play("BlackLeft");
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            PlaySound(20);

            keys[13].GetComponent<Animator>().Play("WhiteLeft");
        }


        // Key V and V UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                PlaySound(53);

                keys[8].GetComponent<Animator>().Play("BlackMid");
            }
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            PlaySound(52);

            keys[11].GetComponent<Animator>().Play("WhiteMidLeft");
        }


        // Key B and B UP
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                PlaySound(19);

                keys[9].GetComponent<Animator>().Play("BlackRight");
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            PlaySound(18);

            keys[12].GetComponent<Animator>().Play("WhiteMidRight");
        }

        #endregion

        #region Whites Keys Only
        // Key 3
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaySound(5);

            keys[37].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key 7
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlaySound(12);

            keys[3].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key 0
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlaySound(0);

            keys[52].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key R
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlaySound(46);

            keys[24].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key U
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlaySound(51);

            keys[47].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key Q -
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlaySound(17);

            keys[17].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key F
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlaySound(26);

            keys[57].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key K
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlaySound(35);

            keys[31].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key X
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlaySound(56);

            keys[42].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key N
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlaySound(39);

            keys[10].GetComponent<Animator>().Play("WhiteRight");
        }

        // Key M
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlaySound(38);

            keys[60].GetComponent<Animator>().Play("White");
        }

        #endregion
    }

    /// <summary>
    /// Sheets Displaying Management
    /// </summary>
    private void SheetDisplay()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Displaying sheets UI [on/off] if spacebar is pressed
            baseSheet.SetActive(!state);
            state = !state;
        }

        if (baseSheet.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) NextSheet();
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) PreviousSheet();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Displaying personnal sheets [on/off]
            personalSheet.SetActive(!state2);
            state2 = !state2;
        }
    }

    /// <summary>
    /// Go to next music sheet
    /// </summary>
    public void NextSheet()
    {
        if (actualSheet < sheets.Length - 1)
        {
            baseSheet.GetComponentInChildren<Image>().sprite = null;

            actualSheet++;

            baseSheet.GetComponentInChildren<Image>().sprite = sheets[actualSheet];
        }
        else if (actualSheet >= sheets.Length - 1)
        {
            baseSheet.GetComponentInChildren<Image>().sprite = null;

            actualSheet = 0;

            baseSheet.GetComponentInChildren<Image>().sprite = sheets[actualSheet];
        }
    }

    /// <summary>
    /// Go to previous music sheet
    /// </summary>
    public void PreviousSheet()
    {
        if (actualSheet > 0)
        {
            baseSheet.GetComponentInChildren<Image>().sprite = null;

            actualSheet--;

            baseSheet.GetComponentInChildren<Image>().sprite = sheets[actualSheet];
        }
        else if (actualSheet <= 0)
        {
            baseSheet.GetComponentInChildren<Image>().sprite = null;

            actualSheet = sheets.Length - 1;

            baseSheet.GetComponentInChildren<Image>().sprite = sheets[actualSheet];
        }
    }

    /// <summary>
    /// Increase or decrease pinao volume | Localy effect only
    /// </summary>
    private void Volume()
    {
        audioSource.volume = actualVolume;

        if (actualVolume < 0.1f)
        {
            volAmount.text = "0 %";
        }
        else { volAmount.text = string.Format("{0:#} %", audioSource.volume * 100); }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (actualVolume < 0.91f) actualVolume += 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (actualVolume >= 0.1f) actualVolume -= 0.1f;
        }
    }
    #endregion

    #region Multiplayer Integration
    /// <summary>
    /// Sert à jouer un son en renseignant un id
    /// </summary>
    /// <param name="id"></param>
    public void PlaySound(int id)
    {
        if (id >= 0 && id < notes.Length)
        {
            SoundIDServerRpc(id);
        }
    }

    /// <summary>
    /// Sert à envoyer au serveur l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ServerRpc(RequireOwnership = false)]
    public void SoundIDServerRpc(int id)
    {
        SoundIDClientRpc(id);
    }

    /// <summary>
    /// Sert à envoyer au client l'ID du son
    /// </summary>
    /// <param name="id"></param>
    [ClientRpc]
    public void SoundIDClientRpc(int id)
    {
        audioSource.PlayOneShot(notes[id]);
    }
    #endregion
}