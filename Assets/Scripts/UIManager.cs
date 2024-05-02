using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager UIInstance;

    [Header("Panel Infos")]
    public GameObject panelCustomization;
    public GameObject panelLobbyMenu;
    public GameObject panelCreateLobby;
    public GameObject panelInsideLobby;
    public GameObject camUI;
    
    [Header("Win Section")]
    public GameObject panelEndGame;
    public TextMeshProUGUI winText;

    [Header("Shooting Range")]
    public GameObject panelCrosshair;

    [Header("Customization")]
    [Tooltip("Les cases vides de base")] [SerializeField] private Image[] casesImages = new Image[6];
    [HideInInspector] public CaseInfo caseInfo;
    private Sprite baseSprite;
    private int N;
    private int U;
    private int clicCount;
    private int prevType;
    private int prevN;
    private GameObject prevSel;
    private Color unValidateCol = new Color(0, 255, 45, 255);

    [SerializeField] private GameObject scrollerType;

    [Header("Type Left Panel")]
    private int typeNum;
    private int actualIncrementation;
    private GameObject itemSelected;
    private Sprite itemSelectedSprite;

    [Tooltip("Nom de la catégorie")] [SerializeField] private List<string> typeNames;
    [SerializeField] private TextMeshProUGUI actualTypeName;

    [SerializeField] private Sprite[] sexeSprites = new Sprite[2]; // 0 = homme et 1 = femme // TYPE 0 -> on change juste les presets entre les 3 persos de base
    [SerializeField] private Sprite[] skinColorSprites; // TYPE 1 -> ça dégagera, faire attention à ce que ça pète rien
    [SerializeField] private Sprite[] legSprites; // TYPE 2
    [SerializeField] private Sprite[] bodySprites; // TYPE 3
    [SerializeField] private Sprite[] beardSprites; // TYPE 4
    [SerializeField] private Sprite[] hairAndHatSprites; // TYPE 5
    [SerializeField] private Sprite[] maskSprites; // TYPE 6
    [SerializeField] private Sprite[] utilitySprites; // TYPE 7

    [Header("Real Items")]
    [SerializeField] private GameObject[] sexeCharacter;
    [SerializeField] private Material[] skinColorMat;
    [SerializeField] private GameObject[] legItems;
    [SerializeField] private GameObject[] bodyItems;
    [SerializeField] private GameObject[] beardItems;
    [SerializeField] private GameObject[] hairAndHatItems;
    [SerializeField] private GameObject[] maskItems;
    [SerializeField] private GameObject[] utilityItems;

    [Header("Validation Right Panel")]
    [SerializeField] private List<Image> selectedItemsPictures = new List<Image>(8); // Image.Color à mettre en valeur si n'est pas définitevement validé par le joueur
    [Tooltip("Contient les accessoires sauvegardé du joueur")] [SerializeField] private List<GameObject> savedSelection;

    private PlayerCustomInfo playerCustomInfo;
    #endregion

    #region Built In Methods
    private void Awake()
    {
        if (!UIInstance) UIInstance = this;
    }

    private void Start()
    {
        playerCustomInfo = FindObjectOfType<PlayerCustomInfo>();

        baseSprite = casesImages[0].sprite;

        Init();
    }
    #endregion


    #region Customs Methods

    #region GENERAL
    /// <summary>
    /// Retour vers le menu principal
    /// </summary>
    public void BackToMenu()
    {
        Debug.Log("En avant Marty, retournons vers le menu !");
    }

    private void Init()
    {
        typeNum = 3;
        actualTypeName.text = typeNames[typeNum];

        scrollerType.SetActive(false);

        for (int i = 0; i < casesImages.Length && i < bodySprites.Length; i++)
        {
            casesImages[i].sprite = bodySprites[i];
        }
    }

    private void ResetCase()
    {
        for (int i = 0; i < casesImages.Length; i++)
        {
            casesImages[i].sprite = baseSprite;
        }
    }

    #endregion


    #region LEFT PANEL
    /// <summary>
    /// Aller à la catégorie précédante
    /// </summary>
    public void PreviousType()
    {
        // Catégorie précédente (on vérifie qu'on ne soit pas déjà à la première)
        // On affiche dans les cases respectives, les 6 premiers accessoires disponible sur la liste et on masque les précédents

        // On décrémente de 1 le type d'accesoire
        actualIncrementation = 0;

        typeNum--;

        if (typeNum == -1)
        {
            typeNum = 7;
        }

        actualTypeName.text = typeNames[typeNum];

        clicCount = 0;

        // On affiche les items
        if (typeNum == 0)
        {
            ResetCase();

            ////sexeSprites
            //
            //for (int i = 0; i < casesImages.Length && i < sexeSprites.Length; i++)
            //{
            //    casesImages[i].sprite = sexeSprites[i];
            //}
        }
        else if (typeNum == 1)
        {
            ResetCase();

            if (skinColorSprites.Length <= 6) scrollerType.SetActive(false);
            else if (skinColorSprites.Length > 6) scrollerType.SetActive(true);

            ////skinColorSprites
            //
            //for (int i = 0; i < casesImages.Length && i < skinColorSprites.Length; i++)
            //{
            //    casesImages[i].sprite = skinColorSprites[i];
            //}
        }
        else if (typeNum == 2)
        {
            ResetCase();

            if (legSprites.Length <= 6) scrollerType.SetActive(false);
            else if (legSprites.Length > 6) scrollerType.SetActive(true);

            ////legSprites
            //
            //for (int i = 0; i < casesImages.Length && i < legSprites.Length; i++)
            //{
            //    casesImages[i].sprite = legSprites[i];
            //}
        }
        else if (typeNum == 3)
        {
            ResetCase();

            if (bodySprites.Length <= 6) scrollerType.SetActive(false);
            else if (bodySprites.Length > 6) scrollerType.SetActive(true);

            //bodySprites

            for (int i = 0; i < casesImages.Length && i < bodySprites.Length; i++)
            {
                casesImages[i].sprite = bodySprites[i];
            }
        }
        else if (typeNum == 4)
        {
            ResetCase();

            if (beardSprites.Length <= 6) scrollerType.SetActive(false);
            else if (beardSprites.Length > 6) scrollerType.SetActive(true);

            //beardSprites

            for (int i = 0; i < casesImages.Length && i < beardSprites.Length; i++)
            {
                casesImages[i].sprite = beardSprites[i];
            }
        }
        else if (typeNum == 5)
        {
            ResetCase();

            if (hairAndHatSprites.Length <= 6) scrollerType.SetActive(false);
            else if (hairAndHatSprites.Length > 6) scrollerType.SetActive(true);

            //hairAndHatSprites

            for (int i = 0; i < casesImages.Length && i < hairAndHatSprites.Length; i++)
            {
                casesImages[i].sprite = hairAndHatSprites[i];
            }
        }
        else if (typeNum == 6)
        {
            ResetCase();

            if (maskSprites.Length <= 6) scrollerType.SetActive(false);
            else if (maskSprites.Length > 6) scrollerType.SetActive(true);

            //maskSprites

            for (int i = 0; i < casesImages.Length && i < maskSprites.Length; i++)
            {
                casesImages[i].sprite = maskSprites[i];
            }
        }
        else if (typeNum == 7)
        {
            ResetCase();

            if (utilitySprites.Length <= 6) scrollerType.SetActive(false);
            else if (utilitySprites.Length > 6) scrollerType.SetActive(true);

            //utilitySprites

            for (int i = 0; i < casesImages.Length && i < utilitySprites.Length; i++)
            {
                casesImages[i].sprite = utilitySprites[i];
            }
        }
    }

    /// <summary>
    /// Aller à la catégorie suivante
    /// </summary>
    public void NextType()
    {
        // Catégorie suivante (on vérifie qu'on ne soit pas déjà à la dernière)
        // On affiche dans les cases respectives, les 6 premiers accessoires disponible sur la liste et on masque les précédents

        // On incrémente de 1 le type d'accesoire
        actualIncrementation = 0;

        typeNum++;

        if (typeNum == 8)
        {
            typeNum = 0;
        }

        actualTypeName.text = typeNames[typeNum];

        clicCount = 0;

        // On affiche les items
        if (typeNum == 0)
        {
            ResetCase();

            ////sexeSprites
            //
            //for (int i = 0; i < casesImages.Length && i < sexeSprites.Length; i++)
            //{
            //    casesImages[i].sprite = sexeSprites[i];
            //}
        }
        else if (typeNum == 1)
        {
            ResetCase();

            if (skinColorSprites.Length <= 6) scrollerType.SetActive(false);
            else if (skinColorSprites.Length > 6) scrollerType.SetActive(true);

            ////skinColorSprites
            //
            //for (int i = 0; i < casesImages.Length && i < skinColorSprites.Length; i++)
            //{
            //    casesImages[i].sprite = skinColorSprites[i];
            //}
        }
        else if (typeNum == 2)
        {
            ResetCase();

            if (legSprites.Length <= 6) scrollerType.SetActive(false);
            else if (legSprites.Length > 6) scrollerType.SetActive(true);

            ////legSprites
            //
            //for (int i = 0; i < casesImages.Length && i < legSprites.Length; i++)
            //{
            //    casesImages[i].sprite = legSprites[i];
            //}
        }
        else if (typeNum == 3)
        {
            ResetCase();

            if (bodySprites.Length <= 6) scrollerType.SetActive(false);
            else if (bodySprites.Length > 6) scrollerType.SetActive(true);

            //bodySprites

            for (int i = 0; i < casesImages.Length && i < bodySprites.Length; i++)
            {
                casesImages[i].sprite = bodySprites[i];
            }
        }
        else if (typeNum == 4)
        {
            ResetCase();

            if (beardSprites.Length <= 6) scrollerType.SetActive(false);
            else if (beardSprites.Length > 6) scrollerType.SetActive(true);

            //beardSprites

            for (int i = 0; i < casesImages.Length && i < beardSprites.Length; i++)
            {
                casesImages[i].sprite = beardSprites[i];
            }
        }
        else if (typeNum == 5)
        {
            ResetCase();

            if (hairAndHatSprites.Length <= 6) scrollerType.SetActive(false);
            else if (hairAndHatSprites.Length > 6) scrollerType.SetActive(true);

            //hairAndHatSprites

            for (int i = 0; i < casesImages.Length && i < hairAndHatSprites.Length; i++)
            {
                casesImages[i].sprite = hairAndHatSprites[i];
            }
        }
        else if (typeNum == 6)
        {
            ResetCase();

            if (maskSprites.Length <= 6) scrollerType.SetActive(false);
            else if (maskSprites.Length > 6) scrollerType.SetActive(true);

            //maskSprites

            for (int i = 0; i < casesImages.Length && i < maskSprites.Length; i++)
            {
                casesImages[i].sprite = maskSprites[i];
            }
        }
        else if (typeNum == 7)
        {
            ResetCase();

            if (utilitySprites.Length <= 6) scrollerType.SetActive(false);
            else if (utilitySprites.Length > 6) scrollerType.SetActive(true);

            //utilitySprites

            for (int i = 0; i < casesImages.Length && i < utilitySprites.Length; i++)
            {
                casesImages[i].sprite = utilitySprites[i];
            }
        }
    }

    /// <summary>
    /// Remonter la liste d'items 
    /// </summary>
    public void ItemsUp()
    {
        // Vérifier à ce qu'on ne soit pas déjà en haut de la page
        // On affiche les 6 items du dessus

        if (typeNum == 5 && actualIncrementation > 0)
        {
            ResetCase();

            actualIncrementation -= 6;

            for (int i = 0; i < casesImages.Length && actualIncrementation + i < hairAndHatSprites.Length; i++)
            {
                casesImages[i].sprite = hairAndHatSprites[actualIncrementation + i];
            }
        }
        else if (typeNum == 6 && actualIncrementation > 0)
        {
            ResetCase();

            actualIncrementation -= 6;

            for (int i = 0; i < casesImages.Length && actualIncrementation + i < maskSprites.Length; i++)
            {
                casesImages[i].sprite = maskSprites[actualIncrementation + i];
            }
        }
    }

    /// <summary>
    /// Descendre la liste d'items
    /// </summary>
    public void ItemsDown()
    {
        // Vérifier à ce qu'on en soit pas déjà en bas de la page
        // On affiche les 6 items du dessous

        if (typeNum == 5)
        {
            var A = (int)(hairAndHatSprites.Length / 6); // permet d'éviter de scroller à l'infini

            if (actualIncrementation + A < hairAndHatSprites.Length)
            {
                ResetCase();

                actualIncrementation += 6;

                for (int i = 0; i < casesImages.Length && actualIncrementation + i < hairAndHatSprites.Length; i++)
                {
                    casesImages[i].sprite = hairAndHatSprites[actualIncrementation + i];
                }
            }
        }
        else if (typeNum == 6)
        {
            var A = (int)(maskSprites.Length / 6); // permet d'éviter de scroller à l'infini

            if (actualIncrementation + A < maskSprites.Length)
            {
                ResetCase();

                actualIncrementation += 6;

                for (int i = 0; i < casesImages.Length && actualIncrementation + i < maskSprites.Length; i++)
                {
                    casesImages[i].sprite = maskSprites[actualIncrementation + i];
                }
            }
        }
    }

    /// <summary>
    /// Affiche l'item sélectionné sur le personnage et l'équipe
    /// </summary>
    public void ShowOnCharacterItem()
    {
        // N détermine de quelle case il s'agit
        N = caseInfo.caseId;

        if (actualIncrementation > N)
        {
            N += actualIncrementation;
        }


        if (prevSel == null && clicCount == 1) clicCount = 0;
        else if (clicCount >= 2) clicCount = 1;
        else clicCount++;


        // L'objet est de la même catégorie que le précédent, provient de la même case et à été cliqué deux fois
        if (clicCount == 2 && prevSel != null && prevType == typeNum && prevN == N) // BUG A FIX ICI
        {
            // On sauvegarde le sprite et le gameobjet sélectionné

            //itemSelected
            selectedItemsPictures[typeNum].sprite = itemSelectedSprite;
            selectedItemsPictures[typeNum].color = unValidateCol;

            //savedSelection
            savedSelection[typeNum] = itemSelected;
            savedSelection[typeNum].SetActive(true);

            clicCount = 0;
        }
        else
        {
            if (itemSelected != null)
            {
                itemSelected.SetActive(false);
                itemSelected = null;
            }
            if (itemSelectedSprite != null) itemSelectedSprite = null;

            // On affiche par défaut les skins validé
            for (int i = 0; i < savedSelection.Capacity; i++)
            {
                if (savedSelection[i] != null) savedSelection[i].SetActive(true);
            }
        }


        // Si le type précédent est identique, alors on retire l'accessoire sauvegarder pour ne pas en avoir deux d'affiché
        if (typeNum == prevType && N != prevN)
        {
            if (savedSelection[typeNum] != null)
            {
                savedSelection[typeNum].SetActive(false);
            }
        }

        // Ultime Verif
        if (typeNum == prevType && prevSel != null)
        {
            // Si le type et identique et qu'il y a un item de mis en cache

            if (itemSelected != null) itemSelected.SetActive(false);
        }

        if (savedSelection[typeNum] != null) savedSelection[typeNum].SetActive(false);


        if (typeNum == 0)
        {
            // Pas intégré
        }
        else if (typeNum == 1)
        {
            // Pas intégré
        }
        else if (typeNum == 2)
        {
            // Pas intégré
        }
        else if (typeNum == 3)
        {
            //bodyItems;
            if (N < bodyItems.Length)
            {
                itemSelected = bodyItems[N];

                itemSelected.SetActive(true);
                itemSelectedSprite = bodySprites[N];
            }
            else { itemSelected = null; }
        }
        else if (typeNum == 4)
        {
            //beardItems;
            if (N < beardItems.Length)
            {
                itemSelected = beardItems[N];

                itemSelected.SetActive(true);
                itemSelectedSprite = beardSprites[N];
            }
        }
        else if (typeNum == 5)
        {
            //hairAndHatItems;
            if (N < hairAndHatItems.Length)
            {
                itemSelected = hairAndHatItems[N];

                itemSelected.SetActive(true);
                itemSelectedSprite = hairAndHatSprites[N];
            }
        }
        else if (typeNum == 6)
        {
            //maskItems;
            if (N < maskItems.Length)
            {
                itemSelected = maskItems[N];

                itemSelected.SetActive(true);
                itemSelectedSprite = maskSprites[N];
            }
        }
        else if (typeNum == 7)
        {
            //utilityItems;
            if (N < utilityItems.Length)
            {
                itemSelected = utilityItems[N];

                itemSelected.SetActive(true);
                itemSelectedSprite = utilitySprites[N];
            }
        }

        prevType = typeNum;
        prevN = N;
        prevSel = itemSelected;
    }
    #endregion


    #region RIGHT PANEL
    /// <summary>
    /// Si un objet n'est pas encore sauvegardé, le sauvegarde en cache
    /// </summary>
    public void SaveSelection()
    {
        // défaire les items save des les listes dans déséquiper 

        for (int i = 0; i < savedSelection.Capacity; i++)
        {
            // Pour chaque case non null et encore en couleur (donc non validé)
            if (savedSelection[i] != null && selectedItemsPictures[i].color == unValidateCol)
            {
                playerCustomInfo.itemsValidate[i] = savedSelection[i];
                selectedItemsPictures[i].color = new Color(255, 255, 255, 255);
            }
            else if (savedSelection[i] == null && selectedItemsPictures[i].color == unValidateCol)
            {
                playerCustomInfo.itemsValidate[i] = null;
                selectedItemsPictures[i].color = new Color(255, 255, 255, 255);
            }
        }
    }

    /// <summary>
    /// Déséquiper un accessoire
    /// </summary>
    public void UnequipItem()
    {
        U = caseInfo.caseId;

        if (selectedItemsPictures[U] != null && savedSelection[U] != null)
        {
            if (playerCustomInfo.itemsValidate.Contains(savedSelection[U]))
            {
                var index = playerCustomInfo.itemsValidate.IndexOf(savedSelection[U]);

                selectedItemsPictures[U].sprite = baseSprite;
                selectedItemsPictures[U].color = unValidateCol;

                savedSelection[U].SetActive(false);
                savedSelection[U] = null;
            }
            else
            {
                //itemSelected
                selectedItemsPictures[U].sprite = baseSprite;
                selectedItemsPictures[U].color = new Color(255, 255, 255, 255);

                //savedSelection
                savedSelection[U].SetActive(false);
                savedSelection[U] = null;
            }
        }
    }
    #endregion

    #endregion

    #region UIPanels
    public void PanelOffOnStart()
    {
        panelCustomization.SetActive(false);
        panelLobbyMenu.SetActive(false);
        panelCreateLobby.SetActive(false);
        panelInsideLobby.SetActive(false);
        panelEndGame.SetActive(false);
        panelCrosshair.SetActive(false);
        camUI.SetActive(false);
    }
    #endregion
}