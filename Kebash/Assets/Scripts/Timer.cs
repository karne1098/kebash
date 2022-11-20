using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _timerTextTMP;
  public static Timer Instance;

  private float _maxTimeValue = 10; // Do not change this! It is tied to the music
  private float _timeValue;

  // ================== Methods
  
  void Awake() 
  {
    Instance = this;
    ResetForMain();
  }

  public void ResetForMain()
  {
    _timerTextTMP.color = new Color(0, 0, 0, 0);
    _timeValue = _maxTimeValue;
  }

  void Update()
  {
    if (GameStateManager.Instance.State == GameState.Countdown)
    {
      float newAlpha = Mathf.Lerp(_timerTextTMP.color.a, 0.4f, 0.05f);
      _timerTextTMP.color = new Color(0, 0, 0, newAlpha);
      displayTime(_timeValue);
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
