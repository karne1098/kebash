using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offledgecollision : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Utils.PlayerLayer)
        {
            StartCoroutine(teleport(other));
        }
    }

    private IEnumerator teleport(Collider other)
    {
        yield return new WaitForSeconds(3);
        other.gameObject.transform.position = new Vector3(0, 20, 0);
    }
}
   
