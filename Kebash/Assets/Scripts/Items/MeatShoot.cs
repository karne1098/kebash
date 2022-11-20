using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatShoot : MonoBehaviour
{
  private Rigidbody _rigidBody;
  
  private int _large = 0;

  void Awake()
  {
    _rigidBody = gameObject.GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    _rigidBody.velocity = new Vector3(
      _rigidBody.velocity.x * .8f,
      0,
      _rigidBody.velocity.z * .8f);

    if (_large < 6)
    {
      _large += 1;

      transform.localScale = new Vector3(
        transform.localScale.x * 1.4f,
        transform.localScale.y,
        transform.localScale.z * 1.2f);
    } 
  }
}
