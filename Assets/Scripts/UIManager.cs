using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager UIInstance;

    [Tooltip("Les cases vides de base")] [SerializeField] private Image[] casesImages = new Image[6];
    private Sprite baseSprite;
    [SerializeField] private GameObject scrollerType;

    [Header("Type Left Panel")]
    private int typeNum;
    private int actualIncrementation;
    private int itemSelected;

    [Tooltip("Nom de la catégorie")] [SerializeField] private List<string> typeNames;
    [SerializeField] private TextMeshProUGUI actualTypeName;

    [SerializeField] private Sprite[] sexeSprites = new Sprite[2]; // 0 = homme et 1 = femme // TYPE 0
    [SerializeField] private Sprite[] skinColorSprites; // TYPE 1
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
    #endregion

    #region Built In Methods
    private void Awake()
    {
        if (!UIInstance) UIInstance = this;
    }

    private void Start()
    {
        baseSprite = casesImages[0].sprite;

        Init();
    }

    private void Update()
    {
        Debug.Log(actualIncrementation);
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


        // On affiche les items
        //casesImages[0]
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


        // On affiche les items
        //casesImages[0]
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
            ResetCase();

            actualIncrementation += 6;

            for (int i = 0; i < casesImages.Length && actualIncrementation + i < hairAndHatSprites.Length; i++)
            {
                casesImages[i].sprite = hairAndHatSprites[actualIncrementation + i];
            }
        }
        else if (typeNum == 6)
        {
            ResetCase();

            actualIncrementation += 6;

            for (int i = 0; i < casesImages.Length && actualIncrementation + i < maskSprites.Length; i++)
            {
                casesImages[i].sprite = maskSprites[actualIncrementation + i];
            }
        }
    }

    /// <summary>
    /// Affiche l'item sélectionné sur le personnage
    /// </summary>
    public void ShowOnCharacterItem()
    {
        // OnMouseOver
    }

    /// <summary>
    /// Équiper un accessoire
    /// </summary>
    public void EquipItem()
    {

    }
    #endregion


    #region RIGHT PANEL
    /// <summary>
    /// Si un objet n'est pas encore sauvegardé, le sauvegarde en cache
    /// </summary>
    public void SaveSelection()
    {

    }

    /// <summary>
    /// Déséquiper un accessoire
    /// </summary>
    public void UnequipItem()
    {

    }
    #endregion
    #endregion
}