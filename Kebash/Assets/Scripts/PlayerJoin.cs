using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerJoin : MonoBehaviour
{
    public static PlayerJoin Instance;
    public int CurrentPlayerCount { get; internal set; }

    public Vector3 GetPlayerPosition() {
        return new Vector3(Random.Range(-10, 10), 20, Random.Range(-5, 5));
    }

    void Awake()
    { 
      Instance = this;
    }

    void AddPlayer() {
        CurrentPlayerCount++;
    }
    
    void RemovePlayer() {
        if (CurrentPlayerCount > 0) {
            CurrentPlayerCount--;
        }
    }
}
