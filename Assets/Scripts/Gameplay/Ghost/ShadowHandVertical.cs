using System.Collections;
using Animancer;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShadowHandVertical : MonoBehaviour
{
    [SerializeField] AnimationClip chargeClip;

    [Inject] Player _player;

    AnimancerComponent _animancer;
    SpriteRenderer _renderer;

    Vector3 _intialPosition;
    State _state = State.Inactive;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animancer = GetComponent<AnimancerComponent>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_state != State.Charging) return;

        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth)
            playerHealth.Health -= 1;
    }

    public void Prepare(float prepareSpeed, bool onPlayer)
    {
        _state = State.Preparing;
        if (onPlayer)
            transform.position = _player.transform.position;
        StartCoroutine(CO_Prepare(prepareSpeed));
    }

    IEnumerator CO_Prepare(float prepareSpeed)
    {
        _renderer.enabled = true;

        var finalColor = _renderer.color;
        var t = 0f;
        while (_state == State.Preparing)
        {
            t += Time.deltaTime * prepareSpeed;
            _renderer.color = Color.Lerp(Color.black, finalColor, t);
            yield return null;
        }
    }

    public IEnumerator CO_Charge()
    {
        var state = _animancer.Play(chargeClip);
        yield return state;

        yield return new WaitForSeconds(0.5f);

        state = _animancer.Play(chargeClip);
        state.Speed = -1;
        yield return state;

        _renderer.enabled = false;
    }

    enum State
    {
        Inactive,
        Preparing,
        Charging,
    }
}