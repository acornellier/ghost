using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] BulletsCombatEvent bulletsCombatEvent;

    int _wallLayer;

    bool _moving;
    Vector2 _direction = Vector2.right;

    void Awake()
    {
        _wallLayer = LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        if (bulletsCombatEvent.IsDone)
            gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!_moving) return;

        transform.Translate(speed * Time.deltaTime * _direction);
    }

    public void StartBulletSequence()
    {
        _moving = true;
        bulletsCombatEvent.Run();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == _wallLayer)
            _direction = -_direction;
    }
}