using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPool : MonoBehaviour
{
    public static objectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public List<GameObject> objectsToPool;
    private int amountToPool;

    void OnEnable()
    {
        SharedInstance = this;
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < objectsToPool.Count; i++)
        {
            poolInfo a = objectsToPool[i].GetComponent<poolInfo>();
            for (int j = 0; j < a.poolAmount; j++)
            {
                tmp = Instantiate(objectsToPool[i]);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
    }

    public GameObject GetPooledObject(string Filter)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag.Equals(Filter))
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public void givePooledObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
