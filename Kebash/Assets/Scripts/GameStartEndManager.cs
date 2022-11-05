using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartEndManager : MonoBehaviour
{
  void BroadcastGameStart() {
    BroadcastMessage("OnGameStart", SendMessageOptions.DontRequireReceiver);
  }

  void BroadcastGameEnd() {
    BroadcastMessage("OnGameEnd", SendMessageOptions.DontRequireReceiver);
  }
}
