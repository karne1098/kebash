using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthtrack : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(30.0f, 30.0f);
    private Slider _staminabar;
    private P1 _playerscript;
    
    
    void Awake()
    {
        _staminabar = GetComponent<Slider>();
        _playerscript = player.GetComponent<P1>();
        _staminabar.value = _playerscript.Stamina / 100.0f;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        transform.position = screenPos + offset;
        _staminabar.value = _playerscript.Stamina / 100.0f;
    }
}
