using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    Vector2 _direction;
    float _speed;

    void FixedUpdate()
    {
        transform.Translate(_speed * Time.deltaTime * _direction);
    }

    public void Initialize(Vector2 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var playerHealth = col.GetComponent<PlayerHealth>();
        if (!playerHealth) return;

        playerHealth.Health -= 1;
    }
}