using UnityEngine;
using UnityEngine.Pool;

public class MonoMonoPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] T prefab;

    MonoPool<T> _pool;

    void Awake()
    {
        _pool = new MonoPool<T>(prefab);
    }
}