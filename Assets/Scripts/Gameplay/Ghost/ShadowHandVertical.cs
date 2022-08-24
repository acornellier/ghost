using System.Collections;
using Animancer;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShadowHandVertical : MonoBehaviour
{
    [SerializeField] AnimationClip idleClip;
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
        _animancer.Play(idleClip);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_state != State.Charging) return;

        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth)
            playerHealth.Health -= 1;
    }

    public IEnumerator CO_Prepare(float prepareTime, bool onPlayer)
    {
        _state = State.Preparing;
        if (onPlayer)
            transform.position = _player.transform.position + 2 * Vector3.down;

        _renderer.enabled = true;

        var finalColor = _renderer.color;
        var t = 0f;
        while (t < prepareTime)
        {
            t += Time.deltaTime;
            var color = Color.Lerp(Color.black, finalColor, t / prepareTime);
            color.a = Mathf.Lerp(0, 1, 2 * t / prepareTime);
            _renderer.color = color;
            yield return null;
        }
    }

    public IEnumerator CO_Charge()
    {
        _state = State.Charging;

        var state = _animancer.Play(chargeClip);
        yield return state;

        yield return new WaitForSeconds(0.5f);

        state = _animancer.Play(chargeClip);
        state.Speed = -1;
        yield return state;

        _renderer.enabled = false;

        _state = State.Inactive;
        _animancer.Play(idleClip);

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            var color = _renderer.color;
            color.a = Mathf.Lerp(1, 0, t);
            _renderer.color = color;
            yield return null;
        }
    }

    enum State
    {
        Inactive,
        Preparing,
        Charging,
    }
}