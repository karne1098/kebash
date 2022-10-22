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

  public void OnPlayerJoined(PlayerInput playerInput) {
    CurrentPlayerCount++;
    Debug.Log("Player " + CurrentPlayerCount + " joined!");
    
    GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject obj in objects) {
      PlayerInput input = obj.GetComponent<PlayerInput>();
      if (input == playerInput) {
        Movement movement = obj.GetComponent<Movement>();
        movement.PlayerNumber = CurrentPlayerCount;
        movement.RespawnPosition = GetPlayerPosition();
      }
    }
  }
  
  public void OnPlayerLeft() {
    Debug.Log("MultiplayerManager: OnPlayerLeft() not supported!");
  }
}
