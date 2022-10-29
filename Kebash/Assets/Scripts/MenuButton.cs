using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void RescaleOnPointerEnter()
    {
        gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    public void RescaleOnPointerExit()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
