using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomShoot : MonoBehaviour
{
    public FoodShootBase baseShot;
    public Rigidbody mushroomBody;
    public GameObject bulletPrefab;


    private void CreateBullet(GameObject bulletPrefab, float angleOffset = 0f)
    {
        GameObject bullet = Instantiate<GameObject>(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20f;
        bullet.GetComponent<FoodShootBase>().StartShot(bullet.transform);
        bullet.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * transform.right * 100.0f);
   }

    // Start is called before the first frame update
    void Start()
    {
        CreateBullet(bulletPrefab, -30f);
        CreateBullet(bulletPrefab, -15f);
        CreateBullet(bulletPrefab, 15f);
        CreateBullet(bulletPrefab, 30f);
    }
 

}
