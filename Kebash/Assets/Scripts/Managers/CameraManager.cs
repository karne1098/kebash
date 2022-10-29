using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject[] _players =  new GameObject[4];
    private float _zOffset;
    private Transform _target;
    private int playerCount = 0;

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
        Vector3 averagePos = new Vector3(0f, 0f, 0f);
        int count = 0;
        bool loopFlag = true;
        float averageX = 0f;
        float averageZ = 0f;

        while (loopFlag)
        {
            if (count == playerCount)
            {
                loopFlag = false;
            } else
            { 
                if (_players[count].transform.position.z > 0f)
                {
                    if (_players[count].transform.position.z > 13.75f)
                    {
                        averageZ = 13.75f;
                    }
                    else
                    {
                        averageZ = _players[count].transform.position.z;
                    }
                } else
                {
                    if (_players[count].transform.position.z < -13.75f)
                    {
                        averageZ = -13.75f;
                    }
                    else
                    {
                        averageZ = _players[count].transform.position.z;
                    }
                }

                if (_players[count].transform.position.x > 0f)
                {
                    if (_players[count].transform.position.x > 8.75f)
                    {
                        averageX = 8.75f;
                    }
                    else
                    {
                        averageX = _players[count].transform.position.x;
                    }
                }
                else
                {
                    if (_players[count].transform.position.x < -8.75f)
                    {
                        averageX = -8.75f;
                    }
                    else
                    {
                        averageX = _players[count].transform.position.x;
                    }
                }
                averagePos = new Vector3(averagePos.x + averageX, averagePos.y + _players[count].transform.position.y, averagePos.z + averageZ);
            }
            count = count + 1;
        }

        if (playerCount != 0)
        {
            averagePos = new Vector3(averagePos.x / (float)playerCount, averagePos.y / (float)playerCount, averagePos.z / (float)playerCount);
        }
        
        _target.position =  new Vector3(
            (averagePos.x / 2), 
            _target.position.y, 
            (averagePos.z / 2) + _zOffset);

        Camera.main.transform.position = _target.position;
        
    }

    public void AddPlayer(GameObject newPlayer)
    {
        _players[playerCount] = newPlayer;
        playerCount = playerCount + 1;
    }

}
