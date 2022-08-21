using System;
using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerInteractor interactor;
    [SerializeField] Stats stats;

    [Inject] LevelLoader _levelLoader;

    PlayerInputActions.PlayerActions _actions;
    AnimancerComponent _animancer;
    Collider2D _collider;
    Rigidbody2D _body;

    bool _dashInput;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
        _animancer = GetComponent<AnimancerComponent>();
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        EnableControls();
        health.onHealthChange += HandleHealthChange;
    }

    void OnDisable()
    {
        DisablePlayerControls();
        health.onHealthChange += HandleHealthChange;
    }

    void Start()
    {
        _actions.Interact.performed += (_) => Interact();
        _actions.Dash.performed += (_) => Dash();
        _actions.Reset.performed += (_) => Die();
    }

    void FixedUpdate()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        var newPosition =
            (Vector2)transform.position + stats.moveSpeed * Time.deltaTime * moveInput;

        if (_dashInput)
        {
            newPosition += stats.dashDistance * moveInput;
            _dashInput = false;
        }

        _body.MovePosition(newPosition);
    }

    public void EnableControls()
    {
        _actions.Enable();
    }

    public void DisablePlayerControls()
    {
        _actions.Disable();
    }

    void Interact()
    {
        interactor.Interact();
    }

    void Dash()
    {
        _dashInput = true;
    }

    void HandleHealthChange(float prevHealth, float newHealth)
    {
        if (newHealth <= 0)
        {
            Die();
            return;
        }
    }

    void Die()
    {
        _levelLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [Serializable]
    class Stats
    {
        public float moveSpeed = 8;
        public float dashDistance = 5;
        public float immuneTime = 0.5f;
    }

    [Serializable]
    class Animations
    {
    }
}