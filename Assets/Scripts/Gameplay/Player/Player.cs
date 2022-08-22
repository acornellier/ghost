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
    [SerializeField] Animations animations;

    [Inject] LevelLoader _levelLoader;

    PlayerInputActions.PlayerActions _actions;
    AnimancerComponent _animancer;
    Collider2D _collider;
    Rigidbody2D _body;

    Vector2 _facingDirection;
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
        UpdateMovement();
        UpdateDirection();
        UpdateAnimations();
    }

    void UpdateMovement()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        var newPosition =
            (Vector2)transform.position + stats.moveSpeed * Time.fixedDeltaTime * moveInput;

        if (_dashInput)
        {
            newPosition += stats.dashDistance * moveInput;
            _dashInput = false;
        }

        _body.MovePosition(newPosition);
    }

    void UpdateDirection()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
            _facingDirection = moveInput;
        print(_facingDirection);

        if ((moveInput.x < 0 && transform.localScale.x > 0) ||
            (moveInput.x > 0 && transform.localScale.x < 0))
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    void UpdateAnimations()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();

        PlayDirectionalAnimation(moveInput != Vector2.zero ? animations.walk : animations.idle);
    }

    void PlayDirectionalAnimation(DirectionalAnimationSet animationSet)
    {
        _animancer.Play(animationSet.GetClip(_facingDirection));
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
        public DirectionalAnimationSet idle;
        public DirectionalAnimationSet walk;
    }
}