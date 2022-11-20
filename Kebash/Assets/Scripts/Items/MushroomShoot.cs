using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomShoot : MonoBehaviour
{
  public FoodShootBase baseShot;
  public Rigidbody mushroomBody;
  public GameObject bulletPrefab;

  void Start()
  {
    CreateBullet(bulletPrefab, 2);
  }

  private void CreateBullet(GameObject bulletPrefab, int spread)
  {
    for (int i = 0; i <= spread; i++)
    {
      GameObject bullet = Instantiate<GameObject>(bulletPrefab);
      bullet.transform.position = transform.position;
      bullet.transform.rotation = transform.rotation;
      bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10f;
      float maxSpread = 8f;
      // bullet.GetComponent<FoodShootBase>().StartShot(bullet.transform);
      Vector3 dir = transform.forward + new Vector3(Random.Range(-maxSpread,maxSpread), 0, Random.Range(-maxSpread,maxSpread));
      bullet.GetComponent<Rigidbody>().AddForce(dir * 20f);
      // bullet.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * transform.right * 400.0f);
    }
  }

}
