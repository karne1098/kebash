using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodFall : MonoBehaviour
{
  public float fallSpeed = 0.01f; //how fast it falls
  public float timeout = 10f; //how long before it disappears
  // Start is called before the first frame update

  private bool _dropCheck = false;
  [SerializeField] private ParticleSystem _dropEffect;
  [SerializeField] private ParticleSystem _sparkleEffect;

  private Transform _foodTransform;
  private bool _bobCheck = false;
  private float _bobMin = 0.5f;
  private float _bobMax = 1.0f;
  private float _bobSpeed = 1.5f;
  private float _bobY;

  private Vector3 _startPos;

  void Start()
  {
    _dropEffect    = transform.Find("food drop").gameObject.GetComponent<ParticleSystem>();
    _sparkleEffect = transform.Find("pickup sparkle").gameObject.GetComponent<ParticleSystem>();
    _foodTransform = transform.Find("food mesh").gameObject.GetComponent<Transform>();

    StartCoroutine(Timeout());

    _startPos = transform.position;
  }

  void OnEnable()
  {
    _bobCheck = false;
    _dropCheck = false;
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (transform.position.y > 1)
    {
      transform.Translate(0, -fallSpeed, 0);
    }
    else if (_dropCheck == false)
    {
      _bobCheck = true;
      _dropCheck = true;
      _dropEffect.Play();
      _sparkleEffect.Play();
    }

    if (_bobCheck)
    {
      float bobSin = Mathf.Sin(Time.time * _bobSpeed);
      _bobY = Mathf.Lerp(_bobMax, _bobMin, bobSin / 2.0f + 0.5f);

      _foodTransform.position = new Vector3(_foodTransform.position.x, _bobY, _foodTransform.position.z);

      _foodTransform.Rotate(0f, 1f, 0f, Space.Self);
    }  
  }

  IEnumerator Timeout()
  {
    yield return new WaitForSeconds(timeout);
    this.gameObject.SetActive(false);
    transform.position = _startPos;
  }
}
