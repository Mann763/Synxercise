using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            GameObject obj = poolDictionary[tag].Dequeue();

            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.LogWarning("Object retrieved from pool is null. Tag: " + tag);
            }
        }
        else
        {
            Debug.LogWarning("Object pool does not contain key: " + tag);
        }

        return null;

    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
    }

    public void PreloadObjects(GameObject prefab, int count)
    {
        string tag = prefab.tag;

        if (poolDictionary.ContainsKey(tag))
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                poolDictionary[tag].Enqueue(obj);
            }
        }
    }

    public int GetActiveObjectCount(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            int activeCount = 0;
            foreach (var obj in poolDictionary[tag])
            {
                if (obj.activeSelf)
                {
                    activeCount++;
                }
            }
            return activeCount;
        }

        return 0;
    }
}
