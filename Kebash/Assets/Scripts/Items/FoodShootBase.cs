using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodShootBase : MonoBehaviour
{

    float _bulletSpeed = 20f;

    float _bulletTimer = 1.5f;

    public Rigidbody _bulletBody;
    public Transform _bulletTransform;
    // Start is called before the first frame update

    //foodBullet.GetComponent<FoodShootBase>().StartShot(tip);
    void Start()
    {
        StartCoroutine("BulletKill");
    }

    void FixedUpdate()
    {
        
    }

    private IEnumerator BulletKill()
    {
        yield return new WaitForSeconds(_bulletTimer);
        Destroy(gameObject);
    }

    public void StartShot(Transform tip)
    {
        _bulletBody.velocity = tip.forward * _bulletSpeed;
    }

    public Transform GetTransform()
    {
        return _bulletTransform;
    }
}
