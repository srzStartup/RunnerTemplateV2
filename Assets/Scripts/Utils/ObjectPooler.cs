using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-3)]
public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] private Dictionary<string, Queue<GameObject>> poolDictionary;
    [SerializeField] private List<Pool> pools;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab);
                if (pool.parent)
                    go.transform.SetParent(pool.parent, pool.worldPositionStays);
                go.SetActive(false);
                poolQueue.Enqueue(go);
            }

            poolDictionary.Add(pool.tag, poolQueue);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, bool isActive = true)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject spawned = poolDictionary[tag].Dequeue();

        if (isActive)
            spawned.SetActive(true);
        spawned.transform.SetPositionAndRotation(position, rotation);

        poolDictionary[tag].Enqueue(spawned);

        return spawned;
    }
}

[System.Serializable]
public class Pool
{
    public string tag;
    public int size;
    public GameObject prefab;
    public Transform parent;
    public bool worldPositionStays;
}
