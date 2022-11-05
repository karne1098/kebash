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
  private Image     _fill;

  // Slider opacity management
  private float timeSinceFull;
  private float fadeRate = 0.3f;
  private float beforeFade = 1f;

  private bool _shaking = false;
  private float _shakeDuration = 0.25f;
  private float _shakeAmount = 5;
  private float _fillOpacity = 0;

  void Start()
  {
    _sliderParentTransform = transform.GetChild(0);
    _slider = GetComponentInChildren<Slider>();
    _fill = transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
    _movement = _player.GetComponent<Movement>();

    Vector3 screenPos = Camera.main.WorldToScreenPoint(_player.transform.position);
    _sliderParentTransform.position = screenPos;
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
      _fillOpacity = (fadeRate + beforeFade - (Time.fixedTime - timeSinceFull))/fadeRate;
    }
    else
    {
      _fillOpacity = 1f;
      timeSinceFull = Time.fixedTime;
    }

    if (_movement.StaminaFraction <= 0.2f)
    {
      _fill.color = new Color(1f, 83f/255, 83f/255, _fillOpacity);
    }
    else
    {
      _fill.color = new Color(46f/85, 1f, 46f/85, _fillOpacity);
    }

    if (_shaking){
      Vector3 newPos = _sliderParentTransform.position + Random.insideUnitSphere * _shakeAmount;
      newPos.y = _sliderParentTransform.position.y;
      newPos.z = _sliderParentTransform.position.z;
      _sliderParentTransform.position = newPos;
      Debug.Log("Shaking!");
    }
  }

  public void shake()
  {
    if (_shaking == false){
      StopCoroutine("shakeStamina");
      StartCoroutine("shakeStamina");
    }
  }

  private IEnumerator shakeStamina(){
    Color originalColor = _fill.color;

    if (_shaking == false)
    {
      _shaking = true;
    }

     yield return new WaitForSeconds(_shakeDuration);

    //Stop shaking
    _shaking = false;
    _sliderParentTransform.position = Camera.main.WorldToScreenPoint(_player.transform.position);
    _fill.color = originalColor;
  }
}


/*
if (_shaking)
    {
      Vector3 newPos = _sliderBarTransform.position + Random.insideUnitSphere * _shakeAmount;
      newPos.y = _sliderBarTransform.position.y;
      newPos.z = _sliderBarTransform.position.z;
      _sliderBarTransform.position = newPos;
      _staminaFill.color = Color.red;



        private IEnumerator shakeStamina()
  {
    //Start shaking
    Vector3 originalPos = _sliderBarTransform.position;
    Color originalColor = _staminaFill.color;
    if(_shaking == false){
      _shaking = true;
    }

    yield return new WaitForSeconds(_shakeDuration);

    //Stop shaking
    _shaking = false;
    _sliderBarTransform.position = originalPos;
    _staminaFill.color = originalColor;
  }
    }*/
