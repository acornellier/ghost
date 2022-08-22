using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] NodeEvent nodeEvent;

    int _wallLayer;

    bool _moving;
    Vector2 _direction = Vector2.right;

    void Awake()
    {
        _wallLayer = LayerMask.NameToLayer("Wall");
    }

    void FixedUpdate()
    {
        if (!_moving) return;

        transform.Translate(speed * Time.fixedDeltaTime * _direction);
    }

    public void StartBulletSequence()
    {
        StartCoroutine(CO_RunNodeEvent());
    }

    IEnumerator CO_RunNodeEvent()
    {
        _moving = true;
        yield return nodeEvent.Run();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == _wallLayer)
            _direction = -_direction;
    }
}