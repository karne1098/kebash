using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class DeathCountManager : MonoBehaviour
{
  public static DeathCountManager Instance;

  private int[] killCounts  = new int[4];
  private int[] deathCounts = new int[4];

  [SerializeField] private TextMeshProUGUI topLeft;
  [SerializeField] private TextMeshProUGUI topRight;
  [SerializeField] private TextMeshProUGUI bottomLeft;
  [SerializeField] private TextMeshProUGUI bottomRight;

  void Awake()
  {
    Instance = this;
    ResetForMain();
  }
  
  void Update()
  {
    if(GameStateManager.Instance.State == GameState.Menu) {
    //yep. lazy last minute code. don't think about her too hard. let the if statement just hang out. let it wash through you
      if(MultiplayerManager.Instance.PlayerCount < 4){
        bottomRight.enabled = true;
        bottomRight.text = "Join with a gamepad!";
        if(MultiplayerManager.Instance.PlayerCount < 3){
          bottomLeft.enabled = true;
          bottomLeft.text = "Join with a gamepad!";
          if(MultiplayerManager.Instance.PlayerCount < 2){
            topRight.enabled = true;
            topRight.text = "Join with a gamepad!";
            if(MultiplayerManager.Instance.PlayerCount < 1){
              topLeft.enabled = true;
              topLeft.text = "Join with a gamepad!";
            }
          }
        }
      }
    }
    else{
      disableAllText();
    }
    for (int i = 0; i < deathCounts.Length; ++i)
    {
      if (i < MultiplayerManager.Instance.PlayerCount)
      {
        TextMeshProUGUI textElement = getScorePositionFromIndex(i);
        textElement.enabled = true;
        if (GameStateManager.Instance.State == GameState.Menu)
        {
          textElement.text = "Player " + (i + 1).ToString() + " Joined!";
        }
        else
        {
          textElement.text = killCounts[i].ToString() + " / " + deathCounts[i].ToString();
        }
      }
    }
  }

  public void IncrementKills(int playerIndex)
  {
    killCounts[playerIndex] += 1;
  }

  public void IncrementDeath(int playerIndex)
  {
    deathCounts[playerIndex] += 1;
  }

  public void ResetForMain()
  {
    for (int i = 0; i < deathCounts.Length; ++i)
    {
      killCounts[i]  = 0;
      deathCounts[i] = 0;
    }
  }

  // ================== Helpers

  private TextMeshProUGUI getScorePositionFromIndex(int playerIndex)
  {
    Vector3 vec = MultiplayerManager.Instance.GetPlayerSpawnPosition(playerIndex);

    if (vec[0] <= 0)
    {
      return vec[2] >= 0 ? topLeft : bottomLeft;
    }
    else
    {
      return vec[2] >= 0 ? topRight : bottomRight;
    }
  }

  private void disableAllText()
  {
    topLeft.enabled     = false;
    topRight.enabled    = false;
    bottomLeft.enabled  = false;
    bottomRight.enabled = false;
  }
}
