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

    // Start is called before the first frame update
    void Start()
    {
        _forwardForce = baseShot.GetTransform().forward;
        _currentForce = new Vector3(_forwardForce.x, _forwardForce.y, _forwardForce.z);
        _backwardForce = new Vector3(_forwardForce.x * -1f, _forwardForce.y, _forwardForce.z * -1f);
        _backwardForce = new Vector3(_backwardForce.x * theEggplantEffect, _backwardForce.y, _backwardForce.z * theEggplantEffect);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentForce = _currentForce + _backwardForce;
        eggplantBody.AddForce(_currentForce);
    }
}
