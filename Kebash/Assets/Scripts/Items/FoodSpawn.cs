using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FoodSpawn : MonoBehaviour
{
    public GameObject foodPrefab;
    // for easy editing of variables
    //upper and lower bound for how long before food will appear
    private float minTimeSpawn = 2f;
    private float maxTimeSpawn = 5f;
    //if we can attach this to the size of the field we can make it automatic, for now, we manually change this
    //basically, boundaries for where the food will spawn. 
    private float zSpawn = 17.5f;
    private float xSpawn = 27.5f;

    void Start()
    {
        StartCoroutine(SpawnFood());
    }

    // Update is called once per frame
    IEnumerator SpawnFood()
    {
        while(true)
        {
            float timeRange = Random.Range(minTimeSpawn, maxTimeSpawn);
            float x = (xSpawn * 0.5f) - 0.5f; //not spawning RIGHT on the edge and accounting for field originating at 0,0
            float xRange = Random.Range(-x, x);
            float z = (zSpawn * 0.5f) - 0.5f;
            float zRange = Random.Range(-z, z);
            

            yield return new WaitForSeconds(timeRange);
            Instantiate(foodPrefab, new Vector3(xRange, 15, zRange), Quaternion.identity);
        }
    }
}



