using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CameraManager : MonoBehaviour
{
  public static CameraManager Instance;

  private float _lerpRate = 0.05f;
  private float _dampingFactor = 2f;
  private Vector3 _initialCameraPosition;

  // ================== Accessors

  public Transform CameraTransform { get { return Camera.main.transform; } }

  // ================== Methods

  void Awake()
  {
    Instance = this;

    _initialCameraPosition = CameraTransform.position;
  }

  void FixedUpdate()
  {
    Vector3 averagePos = MultiplayerManager.Instance.GetAveragePlayerPositionForCamera();
    
    Vector3 idealCameraPosition = new Vector3(
      averagePos.x / _dampingFactor, 
      _initialCameraPosition.y, 
      averagePos.z / _dampingFactor + _initialCameraPosition.z);
    
    CameraTransform.position = Vector3.Lerp(
      CameraTransform.position,
      idealCameraPosition,
      _lerpRate);
  }

  public void ResetForMain()
  {
    CameraTransform.position = _initialCameraPosition;
  }
}
