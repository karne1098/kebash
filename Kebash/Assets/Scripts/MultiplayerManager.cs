using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class MultiplayerManager : MonoBehaviour
{
  public static MultiplayerManager Instance;

  public int CurrentPlayerCount { get; internal set; } = 0;

  // ================== Methods

  void Awake()
  { 
    Instance = this;
  }

  public Vector3 GetPlayerPosition() {
    return new Vector3(Random.Range(-10, 10), 20, Random.Range(-5, 5));
  }

  public void OnPlayerJoined() {
    CurrentPlayerCount++;
    Debug.Log("Player " + CurrentPlayerCount + " joined!");

    // Todo: set player number, set player transform position and rotation
  }
  
  public void OnPlayerLeft() {
    Debug.Log("MultiplayerManager: OnPlayerLeft() not supported!");
  }
}
