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

  // ================== Methods

  void Awake()
  {
    Instance = this;
  }

  public void PlayGame()
  {
    if (MultiplayerManager.Instance.PlayerCount == 0) return;

    StartCoroutine("startGame");
  }

  public void QuitGame()
  {
    AudioManager.Instance.Play("vineBoom");
    Application.Quit();
  }

  public void ResetToMenu()
  {
    resetButtonModels();
    _invisibleButtons.SetActive(true);
    _instructionsCreditsOverlays.SetActive(true);
    AudioManager.Instance.Stop("end");
  }

  // ================== Helpers

  private IEnumerator startGame()
  {
    AudioManager.Instance.Stop("bgMusic");
    AudioManager.Instance.Play("intro");

    depressButtonModels();
    _invisibleButtons.SetActive(false);
    _instructionsCreditsOverlays.SetActive(false);

    yield return new WaitForSeconds(4.5f);

    GameStateManager.Instance.UpdateGameState(GameState.Countdown);

    // yield return new WaitForSeconds(10f);
    // GameStateManager.Instance.UpdateGameState(GameState.GamePaused);
  }

  private void depressButtonModels()
  {
    foreach (ButtonAnim x in _buttonModels.GetComponentsInChildren<ButtonAnim>())
    {
      x.Depress();
    }
  }

  private void resetButtonModels()
  {
    foreach (ButtonAnim x in _buttonModels.GetComponentsInChildren<ButtonAnim>())
    {
      x.Reset();
    }
  }
}
