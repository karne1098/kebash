using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FoodSpawn : MonoBehaviour
{
	public static FoodSpawn Instance;

  // Upper and lower bound for how long between food spawns (TODO: tweak)
  private float _maxTimeSpawn = 5f;
  private float _minTimeSpawn = 2f;

  // If we can attach this to the size of the field we can make it automatic, for now, we manually change this
  // basically, boundaries for where the food will spawn. 
  private float _zSpawn = 17.5f;
  private float _xSpawn = 27.5f;
 
  // ================== Methods

  void Awake()
  {
    Instance = this;
  }
  
  void Start()
  {
    StartCoroutine(SpawnFood());
  }

  // ================== Helpers

  IEnumerator SpawnFood()
  {
    while(true)
    {
      if (GameStateManager.Instance.State == GameState.GamePlay) {
        // Select a random food item
        PooledObjectIndex foodIndex = (PooledObjectIndex) Random.Range(1, 7);
        foodIndex = PooledObjectIndex.Mushroom;

        // Decide on a spawn location
        float x = (_xSpawn * 0.5f) - 0.5f; // not spawning RIGHT on the edge and accounting for field originating at 0,0
        float z = (_zSpawn * 0.5f) - 0.5f;
        Vector3 spawnLocation = new Vector3(
          Random.Range(-x, x), 
          15, 
          Random.Range(-z, z));

        // Spawn the food object
        GameObject foodObject = FoodPooler.Instance.GetPooledObject(foodIndex);
        foodObject.transform.position = spawnLocation;
        foodObject.SetActive(true);
        foodObject.GetComponent<FoodData>().Num = foodIndex;

        // Wait to spawn next food item
        float timeToNextSpawn = Random.Range(_minTimeSpawn, _maxTimeSpawn);

        yield return new WaitForSeconds(timeToNextSpawn);
      } else {
        yield return new WaitForSeconds(1);
      }
    }
  }
}
