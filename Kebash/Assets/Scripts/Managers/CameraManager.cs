using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;

    private float _zOffset;
    private Transform _target;

    void Awake()
    {
        _target = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        _zOffset = _target.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 averagePlayerPosition = 
            (_player1.transform.position + _player2.transform.position) / 2;

        _target.position =  new Vector3(
            averagePlayerPosition.x / 2, 
            _target.position.y, 
            averagePlayerPosition.z / 2 + _zOffset);
    }

}
