using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFall : MonoBehaviour
{
    public float fallSpeed = 0.01f; //how fast it falls
    public float timeout = 10f; //how long before it disappears
    // Start is called before the first frame update

    void Start()
    {
      StartCoroutine(Timeout());
    }

    // Update is called once per frame
    void Update()
    {
      if(transform.position.y > 1) {
          transform.Translate(0, -fallSpeed, 0);
      }
    }
    IEnumerator Timeout() {
      yield return new WaitForSeconds(timeout);
      Destroy(this.gameObject);
    }
}
