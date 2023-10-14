using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager UIInstance;

    [SerializeField] private Image[] casesImages = new Image[6];

    [Header("Type Left Panel")]
    private int typeNum;
    [Tooltip("Nom du type d'accesoire")] [SerializeField] private List<string> typeNames;
    [SerializeField] private Image[] sexeImages = new Image[2]; // 0 = homme et 1 = femme
    [SerializeField] private Image[] skinColorImages;
    [SerializeField] private Image[] bodySkinImages;
    [SerializeField] private Image[] legSkinImages;
    [SerializeField] private Image[] beardImages;
    [SerializeField] private Image[] hairAndHatImages;
    [SerializeField] private Image[] maskImages;
    [SerializeField] private Image[] utilityImages;
    #endregion

    #region Built In Methods
    private void Awake()
    {
        if (!UIInstance) UIInstance = this;
    }

    private void Start()
    {
        
    }
    #endregion

    #region Customs Methods

    // GENERAL

    /// <summary>
    /// Retour vers le menu principal
    /// </summary>
    public void BackToMenu()
    {
        Debug.Log("En avant Marty, retournons vers le menu !");
    }


    // LEFT PANEL

    /// <summary>
    /// Aller à la catégorie précédante
    /// </summary>
    public void PreviousType()
    {
        // Catégorie précédente (on vérifie qu'on ne soit pas déjà à la première)
        // On affiche dans les cases respectives, les 6 premiers accessoires disponible sur la liste et on masque les précédents
    }

    /// <summary>
    /// Aller à la catégorie suivante
    /// </summary>
    public void NextType()
    {
        // Catégorie suivante (on vérifie qu'on ne soit pas déjà à la dernière)
        // On affiche dans les cases respectives, les 6 premiers accessoires disponible sur la liste et on masque les précédents
    }

    /// <summary>
    /// Remonter la liste d'items 
    /// </summary>
    public void ItemsUp()
    {
        // Vérifier à ce qu'on ne soit pas déjà en haut de la page
        // On affiche les 6 items du dessus
    }

    /// <summary>
    /// Descendre la liste d'items
    /// </summary>
    public void ItemsDown()
    {
        // Vérifier à ce qu'on en soit pas déjà en bas de la page
        // On affiche les 6 items du dessous
    }


    #endregion
}