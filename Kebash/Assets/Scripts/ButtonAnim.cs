using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
  private Vector3 _hoverDisplacement = new Vector3(0, 0.5f, 0);
  
  public void GetBig()
  {
    transform.position += _hoverDisplacement;
  }

  public void GetSmall()
  {
    transform.position -= _hoverDisplacement;
  }
}
