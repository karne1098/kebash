using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggplantShoot : MonoBehaviour
{
  public FoodShootBase baseShot;
  public Rigidbody eggplantBody;
  public float theEggplantEffect = 1f;

  Vector3 _forwardForce;
  Vector3 _backwardForce;
  Vector3 _currentForce;

  float startY;

  // Start is called before the first frame update
  void Start()
  {
    startY = baseShot.GetTransform().position.y;
    _forwardForce = baseShot.GetTransform().forward;
    _currentForce = new Vector3(_forwardForce.x, 0f, _forwardForce.z);
    _backwardForce = new Vector3(_forwardForce.x * -1f, 0f, _forwardForce.z * -1f);
    _backwardForce = new Vector3(_backwardForce.x * theEggplantEffect, 0f, _backwardForce.z * theEggplantEffect);
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    _currentForce = _currentForce + _backwardForce;
    eggplantBody.AddForce(_currentForce);
    eggplantBody.position = new Vector3(eggplantBody.position.x, startY, eggplantBody.position.z);
  }
}
