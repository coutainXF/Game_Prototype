using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] vFXPools;
    [SerializeField] Pool[] lootItems;

    private static Dictionary<GameObject, Pool> _poolDictionary;

    private void Awake()
    {
        _poolDictionary = new Dictionary<GameObject, Pool>();
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
        Initialize(enemyPools);
        Initialize(lootItems);
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
        CheckPoolSize(enemyPools);
        CheckPoolSize(lootItems);
    }
#endif
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    String.Format("Pool:{0} has a runtime size {1} bigger than setting one {2}"
                    ,pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }

    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (_poolDictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pools! Prefab:"+pool.Prefab.name);
                continue;
            }
#endif
            _poolDictionary.Add(pool.Prefab,pool);
            
            Transform poolParent = new GameObject("Pool"+pool.Prefab.name).transform;
            poolParent.parent = transform;
            
            pool.Initialize(poolParent);
        }
    }
    
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!_poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:"+prefab.name);
            return null;
        }
#endif
        return _poolDictionary[prefab].PreparedObject();
    }
    
    
    
    public static GameObject Release(GameObject prefab,Vector3 position)
    {
#if UNITY_EDITOR
        if (!_poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:"+prefab.name);
            return null;
        }
#endif
        return _poolDictionary[prefab].PreparedObject(position);
    }
    
    
    
    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotaion)
    {
#if UNITY_EDITOR
        if (!_poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:"+prefab.name);
            return null;
        }
#endif
        return _poolDictionary[prefab].PreparedObject(position,rotaion);
    }
    
    
    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotaion,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!_poolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:"+prefab.name);
            return null;
        }
#endif
        return _poolDictionary[prefab].PreparedObject(position,rotaion,localScale);
    }
    
}
