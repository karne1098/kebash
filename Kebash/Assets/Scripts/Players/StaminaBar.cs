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
  private Image _fill;


//Slider opacity management
  private float timeSinceFull;
  private float fadeRate = 0.3f;
  private float beforeFade = 1f;

  void Start()
  {
    _sliderParentTransform = transform.GetChild(0);
    _slider = GetComponentInChildren<Slider>();
    _fill = transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
    _movement = _player.GetComponent<Movement>();

  }

  void FixedUpdate()
  {
    // Update position of slider parent
    Vector3 screenPos = Camera.main.WorldToScreenPoint(_player.transform.position);
    _sliderParentTransform.position = screenPos;

    // Update value of slider
    _slider.value = _movement.StaminaFraction;

    if (_movement.StaminaFraction == 1f)
    {
       Debug.Log("Stamina Full");
      _fill.color = new Color(_fill.color.r, _fill.color.g, _fill.color.b, (fadeRate + beforeFade - (Time.fixedTime - timeSinceFull))/fadeRate);
      
    }
    else{
      Debug.Log("nvm");
       _fill.color = new Color(_fill.color.r, _fill.color.g, _fill.color.b, 1f);
      timeSinceFull = Time.fixedTime;
    }
  }
}
