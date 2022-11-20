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

  void Start()
  {
    GameStateManager.Instance.UpdateGameState(GameState.GamePaused);
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

        Timer            .Instance.ResetForMain();
        CameraManager    .Instance.ResetForMain();
        DeathCountManager.Instance.ResetForMain();
        MainMenuManager  .Instance.ResetForMain();
        
        MultiplayerManager.Instance.ReinitializeAllPlayers();
        MultiplayerManager.Instance.PlayerInputManager.EnableJoining();

        AudioManager.Instance.Play("bgMusic"); 
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
        AudioManager.Instance.Play("gameMusic");
        AudioManager.Instance.Play("sizzle");
        break;
      }
      case GameState.GamePaused:
      {
        Debug.Log("Moved to game pause state.");
        _pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0;
        AudioManager.Instance.Pause("gameMusic");
        AudioManager.Instance.Pause("sizzle");
        break;
      }
      case GameState.GameOver:
      {
        Debug.Log("Moved to game over state.");
        _gameOverCanvas.SetActive(true);
        AudioManager.Instance.Stop("gameMusic");
        AudioManager.Instance.Stop("sizzle");
        AudioManager.Instance.Play("end");
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
  }

  public void Quit()
  {
    Application.Quit();
  }

  // ================== Helpers

  private IEnumerator countdown()
  {
    AudioManager.Instance.Play("countdown");
    yield return new WaitForSeconds(3.5f);
    GameStateManager.Instance.UpdateGameState(GameState.GamePlay);
  }
}
