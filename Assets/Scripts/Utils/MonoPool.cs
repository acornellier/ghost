using UnityEngine;
using UnityEngine.Pool;

public class MonoPool<T> where T : Component
{
    readonly T _prefab;
    readonly Transform _parent;
    readonly ObjectPool<T> _pool;

    public MonoPool(T prefab, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        _pool = new ObjectPool<T>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            100
        );
    }

    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }

    public void Clear()
    {
        _pool.Clear();
    }

    T CreatePooledItem()
    {
        return Object.Instantiate(_prefab, _parent);
    }

    static void OnTakeFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    static void OnReturnedToPool(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    static void OnDestroyPoolObject(T obj)
    {
        Object.Destroy(obj.gameObject);
    }
}