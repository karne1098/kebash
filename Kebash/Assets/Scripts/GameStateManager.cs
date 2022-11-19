using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState : int
{
  Menu,
  Spawning,
  Countdown,
  GamePlay,
  GameOver
}

[DisallowMultipleComponent]
public class GameStateManager : MonoBehaviour
{
  public static GameStateManager Instance;

  // ================== Accessors
  
  public GameState State { get; private set; } = GameState.Menu;

  // ================== Methods
  
  void Awake()
  {
    Instance = this;
  }
  
  public void UpdateGameState(GameState newState)
  {    
    State = newState;

    switch (newState) {
      case GameState.Menu:
      {
        AudioManager.Instance.Play("bgMusic"); 
        break;
      }
      case GameState.Spawning:
      {
        break;
      }
      case GameState.Countdown:
      {
        StartCoroutine("countdown");
        break;
      }
      case GameState.GamePlay:
      {
        AudioManager.Instance.Play("gameMusic");
        break;
      }
      case GameState.GameOver:
      {
        AudioManager.Instance.Stop("gameMusic");
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
