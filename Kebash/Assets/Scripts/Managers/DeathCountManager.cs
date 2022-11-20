using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class DeathCountManager : MonoBehaviour
{
  public static DeathCountManager Instance;

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
    disableAllText();
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
          textElement.text = deathCounts[i].ToString();
        }
      }
    }
  }

  public void incrementDeath(int playerNumber)
  {
    Debug.Log("Player " + playerNumber + "has died!");
    deathCounts[playerNumber] += 1;
  }

  public void ResetForMain()
  {
    for (int i = 0; i < deathCounts.Length; ++i)
    {
      deathCounts[i] = 0;
    }
  }

  // ================== Helpers

  private TextMeshProUGUI getScorePositionFromIndex(int playerNumber)
  {
    Vector3 vec = MultiplayerManager.Instance.GetPlayerSpawnPosition(playerNumber);

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
