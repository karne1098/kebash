using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeValue = 60; 
    public TextMeshProUGUI timeTextTMP;
    public GameObject gameOverTextObject;
    private bool gameStarted = false;

    // ================== Methods
    
    void Update()
    {
      if (GameStateManager.Instance.State == GameState.GamePlay)
      {
        if (timeValue > 0) 
        {
          timeValue -= Time.deltaTime;
        }
        else // no time left
        {
          timeValue = 0; // locks to 0
          gameOverTextObject.SetActive(true);
          GameStateManager.Instance.UpdateGameState(GameState.GameOver);
        }     
      }
      DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
      if (timeToDisplay < 0)
      {
        Debug.Log("done");
        timeToDisplay = 0;   
      }

      //calculating minutes and seconds values
      float minutes = Mathf.FloorToInt(timeToDisplay / 60);
      float seconds = Mathf.FloorToInt(timeToDisplay % 60);

      timeTextTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }    
}
