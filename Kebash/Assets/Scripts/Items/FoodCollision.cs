using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoodCollision : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    // Food only ever interacts with the food picker layer

    // Have the player attempt to take the food
    bool foodWasTaken = 
      other.transform.parent.gameObject.GetComponent<Movement>()
      .AddFood(this.gameObject.GetComponent<FoodData>().Num);

    // Disable self if food was taken
    if (foodWasTaken) this.gameObject.SetActive(false);
  }

}

