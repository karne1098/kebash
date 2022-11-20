using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState : int
{
  NullState,
  Menu,
  Spawning,
  Countdown,
  GamePlay,
  GamePaused,
  GameOver
}

[DisallowMultipleComponent]
public class GameStateManager : MonoBehaviour
{
  public static GameStateManager Instance;

  [SerializeField] private GameObject _pauseMenuCanvas;
  [SerializeField] private GameObject _gameOverCanvas;

  // ================== Accessors
  
  public GameState State { get; private set; } = GameState.NullState;

  // ================== Methods
  
  void Awake()
  {
    Instance = this;
  }

  void Reset(){

  }

  void Start()
  {
    GameStateManager.Instance.UpdateGameState(GameState.Menu);
  }
  
  public void UpdateGameState(GameState newState)
  {
    MultiplayerManager.Instance.PlayerInputManager.DisableJoining();
    Time.timeScale = 1;
    _pauseMenuCanvas.SetActive(false);
    _gameOverCanvas.SetActive(false);

    State = newState;

    switch (newState) {
      case GameState.NullState: { break; }
      case GameState.Menu:
      {
        Debug.Log("Moved to menu state.");
        MultiplayerManager.Instance.PlayerInputManager.EnableJoining();
        AudioManager.Instance.Play("bgMusic", 0); 
        break;
      }
      case GameState.Spawning:
      {
        Debug.Log("Moved to spawning state.");
        break;
      }
      case GameState.Countdown:
      {
        Debug.Log("Moved to countdown state.");
        StartCoroutine("countdown");
        break;
      }
      case GameState.GamePlay:
      {
        Debug.Log("Moved to game play state.");
        Time.timeScale = 1;
        Timer.Instance.Reset();
        AudioManager.Instance.Play("gameMusic", 0);
        AudioManager.Instance.Play("sizzle", 0);
        break;
      }
      case GameState.GamePaused:
      {
        Debug.Log("Moved to game pause state.");
        _pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0;
        AudioManager.Instance.Pause("gameMusic", 0);
        break;
      }
      case GameState.GameOver:
      {
        Debug.Log("Moved to game over state.");
        _gameOverCanvas.SetActive(true);
        AudioManager.Instance.Stop("gameMusic", 0);
        AudioManager.Instance.Stop("sizzle", 0);
        AudioManager.Instance.Play("end", 0);
        break;
      }
    }
  }

  public void Pause()
  {
    UpdateGameState(GameState.GamePaused);
  }

  public void Unpause()
  {
    UpdateGameState(GameState.GamePlay);
  }

  public void GoToMenu()
  {
    UpdateGameState(GameState.Menu);
    MainMenuManager.Instance.ResetToMenu();
    MultiplayerManager.Instance.ReinitializeAllPlayers();
    CameraManager.Instance.ResetToInitialPosition();
  }

  // ================== Helpers

  private IEnumerator countdown()
  {
    AudioManager.Instance.Play("countdown", 0);
    yield return new WaitForSeconds(3.5f);
    GameStateManager.Instance.UpdateGameState(GameState.GamePlay);
  }
}
