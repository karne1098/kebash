using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatShoot : MonoBehaviour
{
    public int large = 0;
    public GameObject flesh;
    public Rigidbody fleshBody;

    void FixedUpdate()
    {
        Vector3 _currentForce = fleshBody.velocity;
        fleshBody.velocity = new Vector3(_currentForce.x * .8f, _currentForce.y, _currentForce.z * .8f);
        if(large < 6){
            float a = flesh.transform.localScale.x;
            flesh.transform.localScale = new Vector3(1.4f * a, flesh.transform.localScale.y, flesh.transform.localScale.z);
            large += 1;
        }
        
    }
}
