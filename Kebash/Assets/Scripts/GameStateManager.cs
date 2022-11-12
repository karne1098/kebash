using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState : int
{
  menu,
  spawn,
  countDown,
  gamePlay,
  gameOver

}



public class GameStateManager : MonoBehaviour
{

    public static GameStateManager Instance;

    public GameState State;

    void Awake(){
        Instance = this;

    }
    void Start(){
        UpdateGameState(GameState.menu);
    }
    
    public void UpdateGameState(GameState newState) {
        
        State = newState;
        switch(newState) {
            case GameState.menu:
            {
            AudioManager.Instance.Play("bgMusic"); 
             break;
            }
            case GameState.spawn:
                break;
            case GameState.countDown: {
                StartCoroutine("countdown");
                break;
                }
            case GameState.gamePlay:
            {
                Debug.Log("musc :P ");
                AudioManager.Instance.Play("gameMusic");
                break;
            }
            case GameState.gameOver:
            {
                AudioManager.Instance.Stop("gameMusic");
                AudioManager.Instance.Play("tacoBell");
                break;
            }
        }
    }

    public GameState getState()
    {
        return State;
    }

    private IEnumerator countdown(){
        AudioManager.Instance.Play("countdown");
        yield return new WaitForSeconds(3.5f);
        GameStateManager.Instance.UpdateGameState(GameState.gamePlay);
    }
}
