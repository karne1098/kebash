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

  [SerializeField] private GameObject _imageCount3;
  [SerializeField] private GameObject _imageCount2;
  [SerializeField] private GameObject _imageCount1;
  [SerializeField] private GameObject _imageFight;
  [SerializeField] private GameObject _pauseMenuCanvas;
  [SerializeField] private GameObject _gameOverCanvas;

  private bool _preventTogglePause = false;

  // ================== Accessors
  
  public GameState State { get; private set; } = GameState.NullState;

  // ================== Methods
  
  void Awake()
  {
    Instance = this;

    _imageCount3.SetActive(false);
    _imageCount2.SetActive(false);
    _imageCount1.SetActive(false);
    _imageFight .SetActive(false);
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

        Timer            .Instance.ResetForMain();
        CameraManager    .Instance.ResetForMain();
        DeathCountManager.Instance.ResetForMain();
        MainMenuManager  .Instance.ResetForMain();
        FoodSpawn        .Instance.ResetForMain();
        
        MultiplayerManager.Instance.ReinitializeAllPlayers();
        MultiplayerManager.Instance.PlayerInputManager.EnableJoining();

        AudioManager.Instance.Play("bgMusic"); 
        break;
      }
      case GameState.Spawning:
      {
        Debug.Log("Moved to spawning state.");
        StartCoroutine("spawning");
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
        Time.timeScale = 0f;
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

  public void TogglePause()
  {
    if (_preventTogglePause) return;

    switch (State)
    {
      case GameState.GamePaused: 
      {
        UpdateGameState(GameState.GamePlay);
        break;
      }
      case GameState.GamePlay: 
      {
        UpdateGameState(GameState.GamePaused);
        break;
      }
      default:
      {
        return;
      }
    }

    StopCoroutine("preventRepeatedPause");
    StartCoroutine("preventRepeatedPause");
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

  private IEnumerator preventRepeatedPause()
  {
    _preventTogglePause = true;
    yield return new WaitForSecondsRealtime(0.5f);
    _preventTogglePause = false;
  }
  
  private IEnumerator spawning()
  {
    AudioManager.Instance.Stop("bgMusic");
    AudioManager.Instance.Play("intro");
    yield return new WaitForSeconds(4.5f);
    
    UpdateGameState(GameState.Countdown);
  }

  private IEnumerator countdown()
  {
    AudioManager.Instance.Play("countdown");

    _imageCount3.SetActive(true);

    yield return new WaitForSeconds(1);
    _imageCount3.SetActive(false);
    _imageCount2.SetActive(true);

    yield return new WaitForSeconds(1);
    _imageCount2.SetActive(false);
    _imageCount1.SetActive(true);

    yield return new WaitForSeconds(1);
    _imageCount1.SetActive(false);
    _imageFight.SetActive(true);

    yield return new WaitForSeconds(1f);
    _imageFight.SetActive(false);
    
    UpdateGameState(GameState.GamePlay);
  }
}
