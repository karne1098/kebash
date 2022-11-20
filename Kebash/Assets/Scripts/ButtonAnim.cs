using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
  private Vector3 _initialPosition;

  private Vector3 _hoverDisplacement = new Vector3(0, 0.5f, 0);
  private Vector3 _hoverPosition;

  [SerializeField] private Vector3 _doneDisplacement;
  private Vector3 _donePosition;


  private float _buttonLerpRate = 0.2f;
  
  // ================== Methods
  
  void Awake()
  {
    _initialPosition = transform.position;
    _hoverPosition   = _initialPosition + _hoverDisplacement;
    _donePosition    = _initialPosition + _doneDisplacement;
  }

  void OnEnable() 
  {
    transform.position = _initialPosition;
  }

  public void Click()
  {
    StopAllCoroutines();
    StartCoroutine(lerpTowards(_donePosition));
  }

  public void PointerEnter()
  {
    StopAllCoroutines();
    StartCoroutine(lerpTowards(_hoverPosition));
  }

  public void PointerExit()
  {
    StopAllCoroutines();
    StartCoroutine(lerpTowards(_initialPosition));
  }

  // ================== Helpers

  private IEnumerator lerpTowards(Vector3 targetPosition)
  {
    while ((transform.position - targetPosition).sqrMagnitude > Mathf.Epsilon)
    {
      transform.position = Vector3.Lerp(
        transform.position,
        targetPosition,
        _buttonLerpRate);
      
      yield return null;
    }
    
  }
}
