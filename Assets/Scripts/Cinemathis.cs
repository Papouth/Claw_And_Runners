using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinemathis : MonoBehaviour
{
    [SerializeField] private string sceneHomeMenu;
    

    private void Update()
    {
        NextScene();
    }

    public void NextScene()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(sceneHomeMenu);
    }
}