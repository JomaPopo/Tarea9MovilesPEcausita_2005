using UnityEngine;
using System.Collections.Generic;

public class StaticObjectPooling : MonoBehaviour
{
    public static StaticObjectPooling Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
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

        for (int i = 0; i < pools.Count; i++)
        {
            Queue<PoolObject> objectPool = new Queue<PoolObject>();

            for (int j = 0; j < pools[i].size; j++)
            {
                GameObject obj = Instantiate(pools[i].prefab);
                PoolObject poolObj = obj.GetComponent<PoolObject>();
                if (poolObj == null)
                {
                    poolObj = obj.AddComponent<PoolObject>();
                }

                poolObj.poolTag = pools[i].tag;
                obj.SetActive(false);
                objectPool.Enqueue(poolObj);
            }

            poolDictionary.Add(pools[i].tag, objectPool);
        }
    }
    public void CreatePool(string tag, GameObject prefab, int size)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " already exists.");
            return;
        }

        Queue<PoolObject> objectPool = new Queue<PoolObject>();

        for (int i = 0; i < size; i++)
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
            Debug.LogWarning("Pool with tag " + tag + " is empty.");
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