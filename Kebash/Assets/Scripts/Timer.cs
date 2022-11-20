using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _timerTextTMP;
  public static Timer Instance;


  public float _timeValue = 60;

  private bool _timerStarted = false;

  // ================== Methods
  
  void Awake() 
  {
    _timerTextTMP.color = new Color(0, 0, 0, 0);
    Instance = this;

  }

  public void Reset(){
    _timeValue = 60;
  }

  void Update()
  {
    if (GameStateManager.Instance.State == GameState.Countdown)
    {
      _timerTextTMP.color = new Color(0, 0, 0, 0.4f);
    }

    if (GameStateManager.Instance.State != GameState.GamePlay) return;
    
    if (_timeValue > 0) 
    {
      _timeValue -= Time.deltaTime;
    }
    else // no time left
    {
      _timeValue = 0;
      GameStateManager.Instance.UpdateGameState(GameState.GameOver);
    }

    displayTime(_timeValue);
  }

  // ================== Helpers

  private void displayTime(float timeToDisplay)
  {
    timeToDisplay = Mathf.Max(0, timeToDisplay);

    // Calculating minutes and seconds
    int minutes = Mathf.FloorToInt(timeToDisplay / 60);
    int seconds = Mathf.FloorToInt(timeToDisplay % 60);

    _timerTextTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
  }    
}
