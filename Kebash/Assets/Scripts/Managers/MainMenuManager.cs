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
  [SerializeField] private GameObject _invisibleButtonsCanvas;
  [SerializeField] private GameObject _instructions1Canvas;
  [SerializeField] private GameObject _instructions2Canvas;
  [SerializeField] private GameObject _creditsCanvas;

  // ================== Methods

  void Awake()
  {
    Instance = this;
  }

  public void PlayGame()
  {
    if (MultiplayerManager.Instance.PlayerCount == 0) return;

    depressButtonModels();

    _invisibleButtonsCanvas.SetActive(false);
    _instructions1Canvas.SetActive(false);
    _instructions2Canvas.SetActive(false);
    _creditsCanvas.SetActive(false);

    GameStateManager.Instance.UpdateGameState(GameState.Spawning);
  }

  public void Quit()
  {
    Application.Quit();
  }

  public void ResetForMain()
  {
    resetButtonModels();

    _invisibleButtonsCanvas.SetActive(true);
    _instructions1Canvas.SetActive(false);
    _instructions2Canvas.SetActive(false);
    _creditsCanvas.SetActive(false);
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
