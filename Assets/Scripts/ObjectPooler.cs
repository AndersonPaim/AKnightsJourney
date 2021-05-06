using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    
   
    ///  Toda classe tem o seu proprio script, evite criar uma nova classe  dentro da outra....
    /// Dito isso, eu até dou uma colher de chá no caso do Object Pooler / Save System  mas para todos os outros casos, crie a classe com seu proprio script.

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int id;
        public int size;
    }

    public List<Pool> pools;

    public Dictionary<int, Queue<GameObject>> poolDictionary;

    private Queue<GameObject> objectPool;

    private GameObject objectToSpawn;

    private bool isPoolAvailable;

    private void Start()
    {
        poolDictionary = new Dictionary<int, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.id, objectPool);
        }
    }

    public GameObject SpawnFromPool(int id)
    {
        objectToSpawn = poolDictionary[id].Dequeue();
        poolDictionary[id].Enqueue(objectToSpawn);

        for(int i = 0; i < pools[id].size; i++)
        {
            GameObject obj = poolDictionary[id].Dequeue();
            poolDictionary[id].Enqueue(obj);

            if (!obj.activeInHierarchy)
            {
                isPoolAvailable = true;
                break;
            }
            else
            {
                isPoolAvailable = false;
            }
        }

        if (!isPoolAvailable)
        {
            AddToPool(id);
        }

        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }

    private void AddToPool(int id)
    {
        GameObject obj = Instantiate(pools[id].prefab);
        objectToSpawn = obj;
        objectPool.Enqueue(obj);
        pools[id].size++;
    }
}
