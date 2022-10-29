using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Loading Main scene!");
        SceneManager.LoadSceneAsync("Scenes/Main");
    }

    public void QuitGame() {
        Debug.Log("Quitting game!");
        Application.Quit();
    }
}
