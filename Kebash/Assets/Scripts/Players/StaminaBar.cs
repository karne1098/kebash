using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
  [SerializeField] private GameObject _player;

  private Transform _sliderParentTransform;
  private Slider    _slider;
  private Movement  _movement;
  
  void Start()
  {
    _sliderParentTransform = transform.GetChild(0);
    _slider = GetComponentInChildren<Slider>();
    _movement = _player.GetComponent<Movement>();
  }

  void FixedUpdate()
  {
    // Update position of slider parent
    Vector3 screenPos = Camera.main.WorldToScreenPoint(_player.transform.position);
    _sliderParentTransform.position = screenPos;

    // Update value of slider
    _slider.value = _movement.StaminaFraction;
  }
}
