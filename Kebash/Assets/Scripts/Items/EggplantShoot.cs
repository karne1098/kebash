using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggplantShoot : MonoBehaviour
{
    public FoodShootBase baseShot;
    public Rigidbody eggplantBody;

    Vector3 _forwardForce;
    Vector3 _backwardForce;

    // Start is called before the first frame update
    void Start()
    {
        _forwardForce = baseShot.GetTransform().forward;
        _backwardForce = new Vector3(_forwardForce.x * -1f, _forwardForce.y, _forwardForce.z * -1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        eggplantBody.AddForce(_backwardForce);
    }
}
