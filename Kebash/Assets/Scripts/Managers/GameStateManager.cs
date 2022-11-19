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
    GameStateManager.Instance.UpdateGameState(GameState.Menu);
  }
  
  public void UpdateGameState(GameState newState)
  {    
    _pauseMenuCanvas.SetActive(false);
    _gameOverCanvas.SetActive(false);

    State = newState;

    switch (newState) {
      case GameState.NullState: { break; }
      case GameState.Menu:
      {
        Debug.Log("moved to menu state");
        AudioManager.Instance.Play("bgMusic"); 
        break;
      }
      case GameState.Spawning:
      {
        break;
      }
      case GameState.Countdown:
      {
        Debug.Log("moved to countdown state");
        StartCoroutine("countdown");
        break;
      }
      case GameState.GamePlay:
      {
        Debug.Log("moved to gamrplay state");
        Time.timeScale = 1;
        AudioManager.Instance.Play("gameMusic");
        AudioManager.Instance.Play("sizzle");
        break;
      }
      case GameState.GamePaused:
      {
        _pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0;
        AudioManager.Instance.Pause("gameMusic");
        break;
      }
      case GameState.GameOver:
      {
        _gameOverCanvas.SetActive(true);
        AudioManager.Instance.Stop("gameMusic");
        AudioManager.Instance.Stop("sizzle");
        AudioManager.Instance.Play("tacoBell");
        break;
      }
    }
  }

  private IEnumerator countdown()
  {
    AudioManager.Instance.Play("countdown");
    yield return new WaitForSeconds(3.5f);
    GameStateManager.Instance.UpdateGameState(GameState.GamePlay);
  }
}
