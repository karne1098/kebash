using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthtrack : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

        offset = new Vector3(30.0f, 30.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        transform.position = screenPos + offset;
    }
}
