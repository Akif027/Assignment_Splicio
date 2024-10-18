using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private Dictionary<GameObject, List<GameObject>> poolDictionary = new Dictionary<GameObject, List<GameObject>>();

    public static ObjectPooler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(GameObject pooledObject, int initialSize)
    {
        if (!poolDictionary.ContainsKey(pooledObject))
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(pooledObject);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary[pooledObject] = objectPool;
        }
    }

    public GameObject GetPooledObject(GameObject pooledObject)
    {
        if (poolDictionary.TryGetValue(pooledObject, out List<GameObject> objectList))
        {
            foreach (GameObject obj in objectList)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            // No inactive objects found, return null instead of instantiating a new one
            Debug.LogWarning($"No available objects in the pool for {pooledObject.name}.");
            return null;
        }

        Debug.LogError($"Pool for {pooledObject.name} does not exist.");
        return null;
    }
    public void ReturnToPool(GameObject objectInstance)
    {
        // Loop through all the pools to find the correct list
        foreach (var pool in poolDictionary.Values)
        {
            if (pool.Contains(objectInstance))
            {
                objectInstance.SetActive(false);
                return;
            }
        }

        Debug.LogError($"{objectInstance.name} is not in any pool.");
    }
}