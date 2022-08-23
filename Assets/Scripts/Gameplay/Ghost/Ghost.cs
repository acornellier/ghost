using System.Collections;
using UnityEngine;
using Zenject;

public class Ghost : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] NodeEvent nodeEvent;
    [SerializeField] AudioClip fightMusic;

    [SerializeField] bool combatOnStart;

    [Inject(Id = "Music")] AudioSource _musicSource;

    int _wallLayer;

    bool _moving;
    Vector2 _direction = Vector2.right;

    void Awake()
    {
        _wallLayer = LayerMask.NameToLayer("Wall");
    }

    void Start()
    {
        if (combatOnStart) StartBulletSequence();
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
        var prevMusic = _musicSource.clip;
        _musicSource.clip = fightMusic;
        _musicSource.Play();
        yield return nodeEvent.Run();
        gameObject.SetActive(false);
        _musicSource.clip = prevMusic;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == _wallLayer)
            _direction = -_direction;
    }
}