using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PooledObjectIndex : int
{
  Null = 0,
  Lamb = 1,
  Onion = 2, 
  Tomato = 3,
}

[System.Serializable]
public class ObjectPoolItem
{
  public GameObject objectToPool;
  public int initialAmount;
}

[DisallowMultipleComponent]
public class FoodPooler : MonoBehaviour
{
	public static FoodPooler Instance;

  [SerializeField] private List<ObjectPoolItem> _itemsToPool;

  private List<List<GameObject>> _pooledObjects;

	// ================== Methods
  
	void Awake()
  {  
    Instance = this; 

    _pooledObjects = new List<List<GameObject>>();
    
    // For each type of thing to pool
    for (int index = 0; index < _itemsToPool.Count; ++index)
    {
      ObjectPoolItem item = _itemsToPool[index];
      _pooledObjects.Add(new List<GameObject>());

      // Pre-instantiate some 
      for (int i = 0; i < item.initialAmount; ++i)
      {
        GameObject obj = Instantiate(item.objectToPool) as GameObject;

        obj.SetActive(false);
        _pooledObjects[index].Add(obj);
      }
    }
  }

  // Note: it's the caller's responsibility to activate the GameObject
  public GameObject GetPooledObject(PooledObjectIndex foodindex)
  {
    int index = (int)foodindex;

    // Return if invalid index
    if (index >= _pooledObjects.Count) return null;

    // Check for existing instance
    for (int i = 0; i < _pooledObjects[index].Count; ++i)
    {
      if (!_pooledObjects[index][i].activeInHierarchy)
      {
        return _pooledObjects[index][i];
      }
    }

    // Otherwise, make a new one
    GameObject obj = Instantiate(_itemsToPool[index].objectToPool) as GameObject;
    obj.SetActive(false);
    _pooledObjects[index].Add(obj);

    return obj;
  }
}