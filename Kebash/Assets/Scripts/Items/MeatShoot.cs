using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatShoot : MonoBehaviour
{

    void FixedUpdate()
    {
        Vector3 _currentForce = transform.GetComponent<Rigidbody>().velocity;
        transform.GetComponent<Rigidbody>().velocity = new Vector3(_currentForce.x * .8f, _currentForce.y, _currentForce.z * .8f);
        
    }
}
