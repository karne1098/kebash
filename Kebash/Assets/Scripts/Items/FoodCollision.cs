using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class FoodCollision : MonoBehaviour
{
  private Rigidbody _rigidbody;

  // ================== Methods

  void Start()
  {
    _rigidbody = GetComponent<Rigidbody>();
  }

  void OnTriggerEnter(Collider other)
  { 
    if (other.gameObject.CompareTag("Player")) {
        bool foodWasTaken = 
          other.transform.parent.gameObject.GetComponent<Movement>()
          .AddFood(this.gameObject.GetComponent<FoodData>().Num);
        if (foodWasTaken) this.gameObject.SetActive(false);
    }
  }

}

