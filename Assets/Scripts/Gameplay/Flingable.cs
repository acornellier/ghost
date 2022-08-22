using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Light2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Flingable : MonoBehaviour
{
    [SerializeField] float liftSpeed = 1;
    [SerializeField] float timeBetweenShakes = 1 / 10f;
    [SerializeField] float shakeAngle = 5;
    [SerializeField] float lightUpTime = 0.5f;
    [SerializeField] float flingSpeed = 50;

    Light2D _light;
    Rigidbody2D _body;

    [Inject] Player _player;

    State _state = State.Inactive;
    Vector2 _flingDirection;

    void Awake()
    {
        _light = GetComponent<Light2D>();
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        switch (_state)
        {
            case State.Inactive:
                return;
            case State.Lifting:
                _body.MovePosition(
                    (Vector2)transform.position + liftSpeed * Time.fixedDeltaTime * Vector2.up
                );
                break;
            case State.Flinging:
                _body.MovePosition(
                    (Vector2)transform.position +
                    flingSpeed * Time.fixedDeltaTime * _flingDirection
                );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth)
            playerHealth.Health -= 1;
    }

    public void Lift()
    {
        _state = State.Lifting;
        StartCoroutine(LightUp());
        StartCoroutine(Shake());
    }

    public void Fling()
    {
        _state = State.Flinging;
        _flingDirection = (_player.transform.position - transform.position).normalized;
    }

    IEnumerator LightUp()
    {
        _light.enabled = true;
        _light.intensity = 0;

        var t = 0f;
        while (_state == State.Lifting)
        {
            t += Time.fixedDeltaTime;
            _light.intensity = Mathf.Lerp(0, 10f, t / lightUpTime);
            yield return null;
        }
    }

    IEnumerator Shake()
    {
        transform.Rotate(0, 0, shakeAngle);
        var angleMultiplier = -1;

        while (_state == State.Lifting)
        {
            transform.Rotate(0, 0, angleMultiplier * 2 * shakeAngle);
            angleMultiplier = -angleMultiplier;
            yield return new WaitForSeconds(timeBetweenShakes);
        }
    }

    enum State
    {
        Inactive,
        Lifting,
        Flinging,
    }
}