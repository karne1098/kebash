using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayAreaManager : MonoBehaviour
{
  public static PlayAreaManager Instance;

  [SerializeField] private GameObject _ground;
  [SerializeField] private GameObject _fallTrigger;

  // ================== Accessors

  void Awake()
  { 
    Instance = this;
  }
}
