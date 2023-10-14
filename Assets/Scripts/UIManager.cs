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


    #endregion
}