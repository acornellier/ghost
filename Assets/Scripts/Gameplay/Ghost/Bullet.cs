using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    LayerMask _collisionMask;
    Rigidbody2D _body;

    MonoPool<Bullet> _pool;
    Vector2 _direction;
    float _speed;

    bool _released;

    void Awake()
    {
        _collisionMask = LayerMask.GetMask("Wall");
    }

    void FixedUpdate()
    {
        transform.Translate(_speed * Time.fixedDeltaTime * _direction);
    }

    public void Initialize(MonoPool<Bullet> pool, Vector2 direction, float speed)
    {
        _released = false;
        _pool = pool;
        _direction = direction;
        _speed = speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            playerHealth.Health -= 0.5f;
            return;
        }

        if (col.gameObject.IsLayerInMask(_collisionMask) && !_released)
        {
            _pool.Release(this);
            _released = true;
        }
    }
}