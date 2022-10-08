using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FoodCollision : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

      void OnTriggerEnter(Collider other)
  { 
    if (other.gameObject.layer == Utils.DamagerLayer){
            //Debug.Log(other.gameObject.GetComponent<Movement>().IsCharging);
            if(other.transform.parent.gameObject.GetComponent<Movement>().IsCharging){
                other.transform.parent.gameObject.GetComponent<Movement>().StartCoroutine("addFood");
                this.gameObject.SetActive(false);
            }
    }
  }

}

