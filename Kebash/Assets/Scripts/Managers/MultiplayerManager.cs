using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class MultiplayerManager : MonoBehaviour
{
  public static MultiplayerManager Instance;

  private float _spawnYOffset = 20;

  // ================== Accessors

  public List<Movement> PlayerScripts { get; private set; } = new List<Movement>();

  public int PlayerCount { get { return PlayerScripts.Count; } }

  // ================== Methods

  void Awake()
  { 
    Instance = this;
  }

  public Vector3 GetAveragePlayerPosition()
  {
    Vector3 averagePosition = Vector3.zero;

    if (PlayerCount == 0) return averagePosition;

    for (int i = 0; i < PlayerCount; ++i)
    {
      averagePosition += GetPlayerPosition(i);
    }

    return averagePosition / PlayerCount;
  }

  // Same, but bounded to screen space
  public Vector3 GetAveragePlayerPositionForCamera()
  {
    Vector3 averagePosition = Vector3.zero;

    if (PlayerCount == 0) return averagePosition;

    for (int i = 0; i < PlayerCount; ++i)
    {
      Vector3 clampedPosition = GetPlayerPosition(i);
      clampedPosition.x = Mathf.Clamp(clampedPosition.x, -13.75f, 13.75f);
      clampedPosition.z = Mathf.Clamp(clampedPosition.z,  -8.75f,  8.75f);
      averagePosition += clampedPosition;
    }

    return averagePosition / PlayerCount;
  }

  public Vector3 GetPlayerPosition(int playerIndex)
  {
    if (playerIndex > PlayerCount) throw new System.Exception("No such player");

    return PlayerScripts[playerIndex].gameObject.transform.position;
  }

  public Vector3 GetPlayerSpawnPosition(int playerIndex)
  {
    switch (PlayerCount)
    {
      case 0: { return new Vector3(0, _spawnYOffset, 0); }
      case 1: { return new Vector3(0, _spawnYOffset, 0); }
      case 2:
      {
        switch (playerIndex)
        {
          case 0:  { return new Vector3(-10, _spawnYOffset,  6); }
          default: { return new Vector3( 10, _spawnYOffset, -6); }
        }
      }
      case 3:
      {
        switch (playerIndex)
        {
          case 0:  { return new Vector3( 0,  _spawnYOffset,  6); }
          case 1:  { return new Vector3(-10, _spawnYOffset, -6); }
          default: { return new Vector3( 10, _spawnYOffset, -6); }
        }
      }
      case 4:
      {
        switch (playerIndex)
        {
          case 0:  { return new Vector3(-10, _spawnYOffset,  6); }
          case 1:  { return new Vector3(-10, _spawnYOffset, -6); }
          case 2:  { return new Vector3( 10, _spawnYOffset,  6); }
          default: { return new Vector3( 10, _spawnYOffset, -6); }
        }
      }
    }
    throw new System.Exception("Unexpected number of players");
  }

  public void OnPlayerJoined(PlayerInput playerInput)
  {
    Movement playerScript = playerInput.gameObject.GetComponent<Movement>();
    PlayerScripts.Add(playerScript);

    ReinitializeAllPlayers();

    Debug.Log("Player has joined.");
  }
  
  public void OnPlayerLeft(PlayerInput playerInput)
  {
    Movement playerScript = playerInput.gameObject.GetComponent<Movement>();
    PlayerScripts.Remove(playerScript);
    
    ReinitializeAllPlayers();

    Debug.Log("Player has left.");
  }

  public void ReinitializeAllPlayers()
  {
    for (int i = 0; i < PlayerCount; ++i)
    {
      Movement playerScript = PlayerScripts[i];

      playerScript.PlayerNumber = i;

      Vector3 spawnPosition = GetPlayerSpawnPosition(i);
      playerScript.RespawnPosition = spawnPosition;

      if (GameStateManager.Instance.State == GameState.Menu)
      {
        playerScript.gameObject.transform.position = spawnPosition;
      }
    }
  }
}
