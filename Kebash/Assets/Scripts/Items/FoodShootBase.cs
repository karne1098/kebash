using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodShootBase : MonoBehaviour
{

    float _bulletSpeed = 20f;

    public Rigidbody _bulletBody;
    public Transform _bulletTransform;
    // Start is called before the first frame update

    //foodBullet.GetComponent<FoodShootBase>().StartShot(tip);
    void Start()
    {

    }

    public void StartShot(Transform tip)
    {
        _bulletBody.velocity = tip.forward * _bulletSpeed;
    }
}
