using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject Prefab;
    public Stack<GameObject> pooled = new();

    public Pool(GameObject prefab)
    {
        Prefab = prefab;
    }
}

public class PoolingManager : Singleton<PoolingManager>
{
    private Dictionary<GameObject, Pool> pools = new();

    public GameObject GetObject(GameObject k, Vector3 pos = default, Quaternion rot = default)
    {
        if (!pools.ContainsKey(k))
        {
            pools.Add(k, new Pool(k));
        }

        Pool pool = pools[k];

        GameObject result;
        if (pool.pooled.Count > 0)
        {
            result = pool.pooled.Pop();
            result.transform.SetParent(null);
            result.transform.SetPositionAndRotation(pos, rot);
            result.gameObject.SetActive(true);
        }
        else
        {
            result = Instantiate(pool.Prefab, pos, rot);
            pools.Add(result, pool);
        }
        return result;
    }

    public T GetObject<T>(GameObject k, Vector3 pos = default, Quaternion rot = default)
    {
        return GetObject(k, pos, rot).GetComponent<T>();
    }

    public void Return(GameObject k)
    {
        if (!pools.ContainsKey(k))
        {
            Destroy(k);
            return;
        }
        Pool pool = pools[k];
        k.transform.SetParent(transform);
        k.SetActive(false);
        pool.pooled.Push(k);
    }
}
