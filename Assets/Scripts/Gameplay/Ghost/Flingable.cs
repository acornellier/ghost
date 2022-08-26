using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Light2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Flingable : MonoBehaviour
{
    [SerializeField] AudioSource liftSource;
    [SerializeField] AudioClip liftClip;

    [SerializeField] AudioSource flingSource;
    [SerializeField] AudioClip flingClip;

    [SerializeField] float liftSpeed = 1;
    [SerializeField] float shakeAngle = 10;
    [SerializeField] float shakeSpeed = 500;
    [SerializeField] float lightUpTime = 0.5f;
    [SerializeField] float lightIntensity = 10f;
    [SerializeField] float flingSpeed = 50;
    [SerializeField] Color lightColor = new(0, 0.4f, 1);
    [SerializeField] bool stayDynamicOnSpawn;

    Light2D _light;
    Collider2D _collider;
    Rigidbody2D _body;
    LayerMask _collisionMask;
    float _initialLiftVolume;

    [Inject] Player _player;

    State _state = State.Inactive;
    float _lastStateTimestamp;

    void Awake()
    {
        _light = GetComponent<Light2D>();
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
        _collisionMask = LayerMask.GetMask("Furniture", "Player", "Wall");
        _initialLiftVolume = liftSource.volume;

        if (!stayDynamicOnSpawn)
            _body.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        var timeSinceState = Time.time - _lastStateTimestamp;

        switch (_state)
        {
            case State.Inactive:
                return;
            case State.Lifting:
                if (timeSinceState < 2f)
                    _body.MovePosition(
                        (Vector2)transform.position + liftSpeed * Time.fixedDeltaTime * Vector2.up
                    );
                break;
            case State.Flinging:
                if (timeSinceState > 5f ||
                    (_body.velocity.magnitude < 0.1f && _body.angularVelocity < 0.1f))
                    Settle();
                break;
            case State.Settling:
                if (timeSinceState > 5f ||
                    (timeSinceState > 1f &&
                     _body.velocity.magnitude < 0.1f && _body.angularVelocity < 0.1f))
                    GoInactive();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_state != State.Flinging) return;

        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth)
            playerHealth.Health -= 1;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (_state == State.Flinging && col.gameObject.IsLayerInMask(_collisionMask))
            Settle();
    }

    public void MakeDynamic()
    {
        _body.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Lift(bool quietly = false)
    {
        _state = State.Lifting;
        _lastStateTimestamp = Time.time;

        liftSource.Stop();
        liftSource.Play();

        liftSource.volume = _initialLiftVolume;
        if (quietly)
            liftSource.volume = 0.1f;

        StartCoroutine(LightUp());
        StartCoroutine(Shake());
    }

    public void Fling()
    {
        _state = State.Flinging;
        _lastStateTimestamp = Time.time;

        StartCoroutine(FadeOutLiftSound());
        flingSource.PlayOneShot(flingClip);

        var direction = (_player.transform.position - transform.position).normalized;
        _body.rotation = default;
        _body.velocity = default;
        _body.AddForce(flingSpeed * direction, ForceMode2D.Impulse);
    }

    public void Drop()
    {
        _body.AddForce(5 * Vector2.down, ForceMode2D.Impulse);
        Settle();
    }

    void Settle()
    {
        _state = State.Settling;
        _lastStateTimestamp = Time.time;
        if (liftSource.isPlaying)
            StartCoroutine(FadeOutLiftSound());
        StartCoroutine(FadeOutLight());
    }

    IEnumerator LightUp()
    {
        _light.enabled = true;
        _light.color = lightColor;
        _light.intensity = 0;

        var t = 0f;
        while (_state == State.Lifting)
        {
            t += Time.fixedDeltaTime;
            _light.intensity = Mathf.Lerp(0, lightIntensity, t / lightUpTime);
            yield return null;
        }

        _light.intensity = lightIntensity;
    }

    IEnumerator Shake()
    {
        _body.SetRotation(Random.Range(-shakeAngle, shakeAngle));

        var angleMultiplier = 1;
        while (_state == State.Lifting)
        {
            var newAngle = transform.rotation.eulerAngles.z +
                           angleMultiplier * shakeSpeed * Time.fixedDeltaTime;
            _body.SetRotation(newAngle);

            if (newAngle > 180) newAngle -= 360;

            if ((angleMultiplier > 0 && newAngle > shakeAngle) ||
                (angleMultiplier < 0 && newAngle < -shakeAngle))
                angleMultiplier = -angleMultiplier;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FadeOutLight()
    {
        var initialIntensity = _light.intensity;
        var t = 0f;
        while (_light.intensity > 0)
        {
            t += Time.deltaTime;
            _light.intensity = Mathf.Lerp(initialIntensity, 0, 2 * t);
            yield return null;
        }

        _light.enabled = false;
    }

    IEnumerator FadeOutLiftSound()
    {
        var initialVolume = liftSource.volume;
        var t = 0f;
        while (liftSource.volume > 0)
        {
            t += Time.deltaTime;
            liftSource.volume = Mathf.Lerp(initialVolume, 0, t);
            yield return null;
        }

        liftSource.Stop();
    }

    void GoInactive()
    {
        _state = State.Inactive;
        // _body.bodyType = RigidbodyType2D.Kinematic;
        _body.velocity = Vector2.zero;
        _body.angularVelocity = 0;
    }

    enum State
    {
        Inactive,
        Lifting,
        Flinging,
        Settling,
    }
}