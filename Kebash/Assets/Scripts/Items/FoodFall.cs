using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFall : MonoBehaviour
{
    public float fallSpeed = 0.01f; //how fast it falls
    public float timeout = 10f; //how long before it disappears
    // Start is called before the first frame update

    bool dropCheck = false;
    ParticleSystem dropEffect;
    ParticleSystem sparkleEffect;

    Transform foodTransform;
    bool bobCheck = false;
    float startingY = 1f;
    float bobMin = 0.5f;
    float bobMax = 1.5f;
    float bobSpeed = 3f;
    float bobY;

    void Start()
    {
        dropEffect = transform.Find("food drop").gameObject.GetComponent<ParticleSystem>();
        sparkleEffect = transform.Find("pickup sparkle").gameObject.GetComponent<ParticleSystem>();

        foodTransform = transform.Find("food mesh").gameObject.GetComponent<Transform>();

        StartCoroutine(Timeout());
    }

    // Update is called once per frame
    void Update()
    {
      if(transform.position.y > 1) {
          transform.Translate(0, -fallSpeed, 0);
      }
      else if(dropCheck == false)
        {
            bobCheck = true;

            dropCheck = true;
            dropEffect.Play();
            sparkleEffect.Play();
        }

      if (bobCheck)
        {
            float bobSin = Mathf.Sin(Time.time * bobSpeed);
            bobY = Mathf.Lerp(bobMax, bobMin, Mathf.Abs(bobSin));

            foodTransform.position = new Vector3(foodTransform.position.x, bobY, foodTransform.position.z);

            foodTransform.Rotate(0f, 1f, 0f, Space.Self);

        }
        
    }
    IEnumerator Timeout() {
      yield return new WaitForSeconds(timeout);
      this.gameObject.SetActive(false);
    }
}
