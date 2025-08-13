using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public int defaultCapacity = 10;
    public int maxPoolSize = 15;
    public GameObject bulletHolePrefab;

    public IObjectPool<GameObject> Pool { get; private set; }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);

        for (int i = 0; i < defaultCapacity; i++)
        {
            var bullet = Pool.Get();
            Pool.Release(bullet);
        }
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(bulletHolePrefab);
        poolGo.GetComponent<BulletHole>().Pool = this.Pool;
        return poolGo;
    }

    // 사용
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}
