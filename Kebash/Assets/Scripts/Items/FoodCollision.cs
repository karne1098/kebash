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
    // Collided with incoming damager
    if (other.gameObject.CompareTag("Player")) {
      // TODO: damager can be charging player, or food projectile. Must disambiguate.
        bool foodWasTaken = 
          other.transform.parent.gameObject.GetComponent<Movement>()
          .AddFood(this.gameObject.GetComponent<FoodData>().Num);
          other.transform.parent.gameObject.GetComponent<Movement>().PlayStab();
        if (foodWasTaken) this.gameObject.SetActive(false);
    }
  }

}

