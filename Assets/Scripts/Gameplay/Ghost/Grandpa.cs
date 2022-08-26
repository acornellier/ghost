using System;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class Grandpa : MonoBehaviour
{
    [SerializeField] AudioClip appearClip;
    [SerializeField] AudioSource appearSource;
    [SerializeField] Animations animations;

    AnimancerComponent _animancer;

    State _state = State.None;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    void FixedUpdate()
    {
        UpdateAnimations();
    }

    public void Appear()
    {
        _state = State.Appearing;
        var state = _animancer.Play(animations.appear);
        state.Events.OnEnd += ResetToIdle;

        appearSource.PlayOneShot(appearClip);
    }

    void UpdateAnimations()
    {
        if (_state == State.None)
            PlayIdleAnimation();
    }

    void ResetToIdle()
    {
        _state = State.None;
        PlayIdleAnimation();
    }

    void PlayIdleAnimation()
    {
        _animancer.Play(animations.idle);
    }

    enum State
    {
        None,
        Appearing,
    }

    [Serializable]
    class Animations
    {
        public AnimationClip idle;
        public AnimationClip appear;
    }
}