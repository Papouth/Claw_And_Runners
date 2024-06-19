using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenuToGame : MonoBehaviour
{
    [SerializeField] private string gameScene;
    [SerializeField] private GameObject panelOptions;
    [SerializeField] private GameObject panelSkins;
    [SerializeField] private GameObject panelHomeMenu;

    // Personnages
    [SerializeField] private GameObject[] persosPrefabs;


    private void Start()
    {
        panelOptions.SetActive(false);
        panelSkins.SetActive(false);
        panelHomeMenu.SetActive(true);

        for (int i = 0; i < persosPrefabs.Length; i++)
        {
            persosPrefabs[i].SetActive(false);
        }
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void SkinsToHomeMenu()
    {
        panelOptions.SetActive(false);
        panelSkins.SetActive(false);
        panelHomeMenu.SetActive(true);

        for (int i = 0; i < persosPrefabs.Length; i++)
        {
            persosPrefabs[i].SetActive(false);
        }
    }

    public void HomeMenuToSkins()
    {
        panelOptions.SetActive(false);
        panelSkins.SetActive(true);
        panelHomeMenu.SetActive(false);

        ShowMaurice();
    }

    public void HomeMenuToOptions()
    {
        panelOptions.SetActive(true);
        panelSkins.SetActive(false);
        panelHomeMenu.SetActive(false);
    }

    public void OptionsToHomeMenu()
    {
        panelOptions.SetActive(false);
        panelSkins.SetActive(false);
        panelHomeMenu.SetActive(true);
    }

    public void ShowMaurice()
    {
        for (int i = 0; i < persosPrefabs.Length; i++)
        {
            persosPrefabs[i].SetActive(false);
        }

        persosPrefabs[0].SetActive(true);
    }

    public void ShowMarcel()
    {
        for (int i = 0; i < persosPrefabs.Length; i++)
        {
            persosPrefabs[i].SetActive(false);
        }

        persosPrefabs[1].SetActive(true);
    }

    public void ShowMichael()
    {
        for (int i = 0; i < persosPrefabs.Length; i++)
        {
            persosPrefabs[i].SetActive(false);
        }

        persosPrefabs[2].SetActive(true);
    }

    public void ShowMichelle()
    {
        for (int i = 0; i < persosPrefabs.Length; i++)
        {
            persosPrefabs[i].SetActive(false);
        }

        persosPrefabs[3].SetActive(true);
    }
}