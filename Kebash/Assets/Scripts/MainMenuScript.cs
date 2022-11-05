using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject toHide;
    public void PlayGame()
    {
        Debug.Log("Loading Main scene!");
        toHide.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("Quitting game!");
        Application.Quit();
    }
}
