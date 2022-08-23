using System;
using System.Collections;
using Animancer;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AnimancerComponent))]
public class Ghost : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] NodeEvent combatNodeEvent;
    [SerializeField] AudioClip fightMusic;
    [SerializeField] bool combatOnStart;
    [SerializeField] Animations animations;

    [Inject] MusicPlayer _musicPlayer;

    AnimancerComponent _animancer;
    int _wallLayer;

    Vector2 _direction = Vector2.right;
    bool _moving;
    AttackState _attackState = AttackState.None;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _wallLayer = LayerMask.NameToLayer("Wall");
    }

    void Start()
    {
        if (combatOnStart) StartCombatSequence();
    }

    void FixedUpdate()
    {
        if (_moving)
            transform.Translate(speed * Time.fixedDeltaTime * _direction);

        UpdateAnimations();
    }

    public void StopAttackingCasting()
    {
        _attackState = AttackState.None;
    }

    public void StartAttacking()
    {
        _attackState = AttackState.Attacking;
        _animancer.Play(animations.attacking);
    }

    public void StartCasting()
    {
        if (_attackState == AttackState.Casting) return;

        _attackState = AttackState.Casting;
        var state = _animancer.Play(animations.cast);
        state.Events.OnEnd += () => _animancer.Play(animations.castLoop);
    }

    void UpdateAnimations()
    {
        if (_attackState == AttackState.None)
            _animancer.Play(animations.idle);
    }

    public void StartCombatSequence()
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
        {
            _direction = -_direction;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    enum AttackState
    {
        None,
        Attacking,
        Casting,
    }

    [Serializable]
    class Animations
    {
        public AnimationClip idle;
        public AnimationClip attacking;
        public AnimationClip cast;
        public AnimationClip castLoop;
        public AnimationClip flyLeft;
    }
}