using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoShoot : MonoBehaviour
{
   public float launchVelocity = 100f;
    void Start()
    {
        transform.GetComponent<Rigidbody>().AddRelativeForce(new Vector3 
                                                (0, launchVelocity,0));
    }

   
}
