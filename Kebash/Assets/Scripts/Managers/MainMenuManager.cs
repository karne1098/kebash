using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class MainMenuManager : MonoBehaviour
{
  public static MainMenuManager Instance;

  [SerializeField] private GameObject _buttonModels;
  [SerializeField] private GameObject _invisibleButtons;
  [SerializeField] private GameObject _instructionsCreditsOverlays;

  public void PlayGame()
  {
    StartCoroutine("startGame");
  }

  public void QuitGame()
  {
    AudioManager.Instance.Play("vineBoom");
    Debug.Log("Quitting game!");
    Application.Quit();
  }

  private IEnumerator startGame()
  {
    AudioManager.Instance.Stop("bgMusic");
    AudioManager.Instance.Play("intro");

    _buttonModels.SetActive(false);
    _invisibleButtons.SetActive(false);
    _instructionsCreditsOverlays.SetActive(false);

    yield return new WaitForSeconds(4.5f);

    GameStateManager.Instance.UpdateGameState(GameState.Countdown);
    FoodSpawn.Instance.StartSpawning();
  }
}
