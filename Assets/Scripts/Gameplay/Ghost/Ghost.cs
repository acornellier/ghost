using System.Collections;
using UnityEngine;
using Zenject;

public class Ghost : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] NodeEvent combatNodeEvent;
    [SerializeField] AudioClip fightMusic;

    [SerializeField] bool combatOnStart;

    [Inject] MusicPlayer _musicPlayer;

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
        StartCoroutine(CO_RunCombatEvent());
    }

    IEnumerator CO_RunCombatEvent()
    {
        _moving = true;

        _musicPlayer.PlayMusic(fightMusic);
        yield return combatNodeEvent.Run();

        gameObject.SetActive(false);
        _musicPlayer.PlayMusic(_musicPlayer.defaultMusic, 1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == _wallLayer)
            _direction = -_direction;
    }
}