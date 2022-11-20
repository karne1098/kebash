using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodShootBase : MonoBehaviour
{
  private float _bulletSpeed = 20f;
  private float _bulletTimer = 7f;

  public Rigidbody _bulletBody;
  public Transform _bulletTransform;

  public int PlayerIndex { get; private set; } = -1;

  void Start()
  {
    StartCoroutine("BulletKill");
  }

  private IEnumerator BulletKill()
  {
    yield return new WaitForSeconds(_bulletTimer);
    Destroy(gameObject);
  }

  public void StartShot(Transform tip, int playerIndex)
  {
    _bulletBody.velocity = tip.forward * _bulletSpeed;
    PlayerIndex = playerIndex;
  }

  public Transform GetTransform()
  {
    return _bulletTransform;
  }
}
