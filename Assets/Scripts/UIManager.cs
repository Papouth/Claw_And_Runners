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

    [Header("Type Left Panel")]
    private int typeNum;
    private int itemSelected;

    [Tooltip("Nom de la cat�gorie")] [SerializeField] private List<string> typeNames;
    [SerializeField] private TextMeshProUGUI actualTypeName;

    //[SerializeField] private Sprite[] sexeSprites = new Sprite[2]; // 0 = homme et 1 = femme // TYPE 0
    //[SerializeField] private Sprite[] skinColorSprites; // TYPE 1
    //[SerializeField] private Sprite[] legSprites; // TYPE 2
    [SerializeField] private Sprite[] bodySprites; // TYPE 3
    [SerializeField] private Sprite[] beardSprites; // TYPE 4
    [SerializeField] private Sprite[] hairAndHatSprites; // TYPE 5
    [SerializeField] private Sprite[] maskSprites; // TYPE 6
    [SerializeField] private Sprite[] utilitySprites; // TYPE 7

    [Header("Real Items")]
    //[SerializeField] private GameObject[] sexeCharacter;
    //[SerializeField] private Material[] skinColorMat; 
    //[SerializeField] private GameObject[] legItems;
    [SerializeField] private GameObject[] bodyItems;
    [SerializeField] private GameObject[] beardItems;
    [SerializeField] private GameObject[] hairAndHatItems;
    [SerializeField] private GameObject[] maskItems;
    [SerializeField] private GameObject[] utilityItems;

    [Header("Validation Right Panel")]
    [SerializeField] private List<Image> selectedItemsPictures = new List<Image>(8); // Image.Color � mettre en valeur si n'est pas d�finitevement valid� par le joueur
    [Tooltip("Contient les accessoires sauvegard� du joueur")] [SerializeField] private List<GameObject> savedSelection;
    #endregion

    #region Built In Methods
    private void Awake()
    {
        if (!UIInstance) UIInstance = this;
    }

    private void Start()
    {
        actualTypeName.text = typeNames[3];

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
    #endregion


    #region LEFT PANEL
    /// <summary>
    /// Aller � la cat�gorie pr�c�dante
    /// </summary>
    public void PreviousType()
    {
        // Cat�gorie pr�c�dente (on v�rifie qu'on ne soit pas d�j� � la premi�re)
        // On affiche dans les cases respectives, les 6 premiers accessoires disponible sur la liste et on masque les pr�c�dents
    }

    /// <summary>
    /// Aller � la cat�gorie suivante
    /// </summary>
    public void NextType()
    {
        // Cat�gorie suivante (on v�rifie qu'on ne soit pas d�j� � la derni�re)
        // On affiche dans les cases respectives, les 6 premiers accessoires disponible sur la liste et on masque les pr�c�dents
    }

    /// <summary>
    /// Remonter la liste d'items 
    /// </summary>
    public void ItemsUp()
    {
        // V�rifier � ce qu'on ne soit pas d�j� en haut de la page
        // On affiche les 6 items du dessus
    }

    /// <summary>
    /// Descendre la liste d'items
    /// </summary>
    public void ItemsDown()
    {
        // V�rifier � ce qu'on en soit pas d�j� en bas de la page
        // On affiche les 6 items du dessous
    }

    /// <summary>
    /// Affiche l'item s�lectionn� sur le personnage
    /// </summary>
    public void ShowOnCharacterItem()
    {
        // OnMouseOver
    }

    /// <summary>
    /// �quiper un accessoire
    /// </summary>
    public void EquipItem()
    {

    }
    #endregion


    #region RIGHT PANEL
    /// <summary>
    /// Si un objet n'est pas encore sauvegard�, le sauvegarde en cache
    /// </summary>
    public void SaveSelection()
    {

    }

    /// <summary>
    /// D�s�quiper un accessoire
    /// </summary>
    public void UnequipItem()
    {

    }
    #endregion
    #endregion
}