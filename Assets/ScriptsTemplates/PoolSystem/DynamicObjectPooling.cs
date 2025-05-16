using UnityEngine;
using System.Collections.Generic;

public class DynamicObjectPooling : MonoBehaviour
{
    public static DynamicObjectPooling Instance { get; private set; }

    private Dictionary<string, Queue<PoolObject>> poolDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        poolDictionary = new Dictionary<string, Queue<PoolObject>>();
    }

    public void CreatePool(string tag, GameObject prefab, int initialSize)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " already exists.");
            return;
        }

        Queue<PoolObject> objectPool = new Queue<PoolObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            PoolObject poolObj = obj.GetComponent<PoolObject>();
            if (poolObj == null)
            {
                poolObj = obj.AddComponent<PoolObject>();
            }

            poolObj.poolTag = tag;
            obj.SetActive(false);
            objectPool.Enqueue(poolObj);
        }

        poolDictionary.Add(tag, objectPool);
    }

    public PoolObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning("Pool with tag " + tag + " is empty. Creating new object.");
            return null;
        }

        PoolObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.OnSpawn();

        return objectToSpawn;
    }

    public void ReturnToPool(PoolObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(objectToReturn.poolTag))
        {
            Debug.LogWarning("Pool with tag " + objectToReturn.poolTag + " doesn't exist.");
            return;
        }

        objectToReturn.OnDespawn();
        poolDictionary[objectToReturn.poolTag].Enqueue(objectToReturn);
    }
}