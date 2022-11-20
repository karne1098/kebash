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

    depressButtonModels();
    _invisibleButtons.SetActive(false);
    _instructionsCreditsOverlays.SetActive(false);

    GameStateManager.Instance.UpdateGameState(GameState.Spawning);
  }

  public void Quit()
  {
    Application.Quit();
  }

  public void ResetForMain()
  {
    resetButtonModels();
    _invisibleButtons.SetActive(true);
    _instructionsCreditsOverlays.SetActive(true);
    AudioManager.Instance.Stop("end");
  }

  // ================== Helpers

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
      x.ResetForMain();
    }
  }
}
