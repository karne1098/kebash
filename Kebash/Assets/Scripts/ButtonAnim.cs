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

  private bool _preventAllOtherCoroutines = false;
  
  // ================== Methods
  
  void Awake()
  {
    _initialPosition = transform.position;
    _hoverPosition   = _initialPosition + _hoverDisplacement;
    _donePosition    = _initialPosition + _doneDisplacement;
  }

  void OnEnable() 
  {
    ResetForMain();
  }

  public void PointerEnter()
  {
    if (_preventAllOtherCoroutines) return;
    StopAllCoroutines();
    StartCoroutine(lerpTowards(_hoverPosition));
  }

  public void PointerExit()
  {
    if (_preventAllOtherCoroutines) return;
    StopAllCoroutines();
    StartCoroutine(lerpTowards(_initialPosition));
  }

  public void Depress()
  {
    _preventAllOtherCoroutines = true;
    StopAllCoroutines();
    StartCoroutine(lerpTowards(_donePosition, true));
  }

  public void ResetForMain()
  {
    transform.position = _initialPosition;
  }

  // ================== Helpers

  private IEnumerator lerpTowards(Vector3 targetPosition, bool isDepressing = false)
  {
    while ((transform.position - targetPosition).sqrMagnitude > 0.01f)
    {
      transform.position = Vector3.Lerp(
        transform.position,
        targetPosition,
        _buttonLerpRate);
      
      yield return null;
    }

    if (isDepressing) { _preventAllOtherCoroutines = false; }
  }
}
