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
    [SerializeField] float shakeAngle = 10;
    [SerializeField] float shakeSpeed = 500;
    [SerializeField] float lightUpTime = 0.5f;
    [SerializeField] float lightIntensity = 10f;
    [SerializeField] float flingSpeed = 50;

    Light2D _light;
    Collider2D _collider;
    Rigidbody2D _body;
    LayerMask _collisionMask;

    [Inject] Player _player;

    State _state = State.Inactive;
    float _settleTime;

    void Awake()
    {
        _light = GetComponent<Light2D>();
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
        _collisionMask = LayerMask.GetMask("Furniture", "Wall");
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
                break;
            case State.Settling:
                var timeSinceSettle = Time.time - _settleTime;
                if (timeSinceSettle > 5f ||
                    (timeSinceSettle > 1f &&
                     _body.velocity.magnitude < 0.1f && _body.angularVelocity < 0.1f))
                    GoInactive();
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (_state == State.Flinging && col.gameObject.IsLayerInMask(_collisionMask))
        {
            D.Log("collided with", col.gameObject.name);
            _state = State.Settling;
            _settleTime = Time.time;
            Physics2D.IgnoreCollision(_collider, _player.GetComponent<Collider2D>(), false);
        }
    }

    public void Lift()
    {
        _state = State.Lifting;
        _body.bodyType = RigidbodyType2D.Dynamic;
        _body.gravityScale = 0;
        _body.drag = 1f;
        _body.angularDrag = 5f;
        StartCoroutine(LightUp());
        StartCoroutine(Shake());
    }

    public void Fling()
    {
        _state = State.Flinging;

        Physics2D.IgnoreCollision(_collider, _player.GetComponent<Collider2D>());
        var direction = (_player.transform.position - transform.position).normalized;
        _body.AddForce(flingSpeed * direction, ForceMode2D.Impulse);
    }

    IEnumerator LightUp()
    {
        _light.enabled = true;
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

    void GoInactive()
    {
        _state = State.Inactive;
        _body.bodyType = RigidbodyType2D.Kinematic;
        _body.velocity = Vector2.zero;
        _body.angularVelocity = 0;
        _light.enabled = false;
    }

    enum State
    {
        Inactive,
        Lifting,
        Flinging,
        Settling,
    }
}