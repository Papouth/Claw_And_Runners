using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenuToGame : MonoBehaviour
{
    [SerializeField] private string gameScene;


    public void GoToGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}