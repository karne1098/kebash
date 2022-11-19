using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
  public Transform modelSize;

  float scaleChange = 0.4f;

  Vector3 scaleBig;
  Vector3 scaleSmall;
  
  void Start()
  {
    scaleBig   = new Vector3(0f, scaleChange,       0f);
    scaleSmall = new Vector3(0f, scaleChange * -1f, 0f);
  }

  public void GetBig()
  {
    modelSize.position += scaleBig;
  }

  public void GetSmall()
  {
    modelSize.position += scaleSmall;
  }
}
