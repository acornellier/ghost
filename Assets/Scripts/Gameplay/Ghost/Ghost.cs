﻿using System;
using System.Collections;
using Animancer;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AnimancerComponent))]
public class Ghost : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] NodeEvent combatNodeEvent;
    [SerializeField] AudioClip fightMusic;
    [SerializeField] bool combatOnStart;
    [SerializeField] Animations animations;

    [Inject] MusicPlayer _musicPlayer;

    AnimancerComponent _animancer;
    int _wallLayer;

    State _state = State.None;

    Vector2 _direction = Vector2.right;

    Vector2 Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            if ((_direction.x > 0 && transform.localScale.x > 0) ||
                (_direction.x < 0 && transform.localScale.x < 0))
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    bool _moving;

    public bool Moving
    {
        get => _moving;
        set
        {
            _moving = value;
            Direction = Vector2.right;
        }
    }

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
        if (Moving)
            transform.Translate(speed * Time.fixedDeltaTime * Direction);

        UpdateAnimations();
    }

    public void StopAttackingCasting()
    {
        _state = State.None;
    }

    public IEnumerator CO_FlyTo(Vector2 target, float flySpeed, bool fly)
    {
        _state = State.Flying;
        Moving = false;
        Direction = (target - (Vector2)transform.position).normalized;

        if (fly)
            _animancer.Play(animations.flyLeft);

        while (Vector2.Distance(transform.position, target) > 0.5f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                flySpeed * Time.deltaTime
            );
            yield return null;
        }

        _animancer.Play(animations.idle);
    }

    public void StartAttacking()
    {
        _state = State.Attacking;
        _animancer.Play(animations.attacking);
    }

    public void StartCasting()
    {
        if (_state == State.Casting) return;

        _state = State.Casting;
        var state = _animancer.Play(animations.cast);
        state.Events.OnEnd += () => _animancer.Play(animations.castLoop);
    }

    void UpdateAnimations()
    {
        if (_state == State.None)
            _animancer.Play(animations.idle);
    }

    public void StartCombatSequence()
    {
        StartCoroutine(CO_RunCombatEvent());
    }

    IEnumerator CO_RunCombatEvent()
    {
        Moving = true;

        _musicPlayer.PlayMusic(fightMusic);
        yield return combatNodeEvent.Run();

        _musicPlayer.PlayMusic(_musicPlayer.defaultMusic, 1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Moving && col.gameObject.layer == _wallLayer)
            Direction = -Direction;
    }

    enum State
    {
        None,
        Flying,
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