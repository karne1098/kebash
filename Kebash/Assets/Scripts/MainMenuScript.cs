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
       StartCoroutine("startGame");
    }

    public void QuitGame() {
        AudioManager.Instance.Play("vineBoom");
        Debug.Log("Quitting game!");
        Application.Quit();
    }

    private IEnumerator startGame()
    {
        Debug.Log("Loading Main scene!"); 
        AudioManager.Instance.Stop("bgMusic");
        AudioManager.Instance.Play("intro");
        yield return new WaitForSeconds(4.5f);
        toHide.SetActive(false);
        GameStateManager.Instance.UpdateGameState(GameState.Countdown);
    }
}
