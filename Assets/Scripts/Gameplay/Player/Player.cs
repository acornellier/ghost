using System;
using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    [SerializeField] Interactors interactors;
    [SerializeField] Stats stats;
    [SerializeField] Animations animations;

    [Inject] LevelLoader _levelLoader;

    PlayerInputActions.PlayerActions _actions;
    AnimancerComponent _animancer;
    Collider2D _collider;
    Rigidbody2D _body;
    SpriteRenderer _renderer;

    Vector2 _facingDirection = Vector2.up;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
        _animancer = GetComponent<AnimancerComponent>();
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
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

        _body.MovePosition(newPosition);
    }

    void UpdateDirection()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        if (moveInput != default)
            _facingDirection = moveInput;

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
        GetInteractor()?.Interact();
    }

    PlayerInteractor GetInteractor()
    {
        if (_facingDirection.x >= 0)
        {
            if (_facingDirection.y >= 0)
                return _facingDirection.x > _facingDirection.y ? interactors.right : interactors.up;

            return _facingDirection.x > -_facingDirection.y ? interactors.right : interactors.down;
        }

        if (_facingDirection.y >= 0)
            return _facingDirection.x < -_facingDirection.y ? interactors.right : interactors.up;

        return _facingDirection.x < _facingDirection.y ? interactors.right : interactors.down;
    }

    void HandleHealthChange(float prevHealth, float newHealth)
    {
        if (newHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(FlashSprite());
    }

    IEnumerator FlashSprite()
    {
        var initialColor = _renderer.color;
        var decreasingAlpha = true;
        while (health.Immune && health.Health > 0)
        {
            var color = _renderer.color;
            color.a += (decreasingAlpha ? -1 : 1) * animations.flashSpeed * Time.deltaTime;
            _renderer.color = color;

            if ((decreasingAlpha && color.a <= 0) || (!decreasingAlpha && color.a >= 1))
                decreasingAlpha = !decreasingAlpha;
            yield return null;
        }

        _renderer.color = initialColor;
    }

    void Die()
    {
        _levelLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [Serializable]
    class Interactors
    {
        public PlayerInteractor up;
        public PlayerInteractor right;
        public PlayerInteractor down;
    }

    [Serializable]
    class Stats
    {
        public float moveSpeed = 8;
    }

    [Serializable]
    class Animations
    {
        public float flashSpeed = 4;
        public DirectionalAnimationSet idle;
        public DirectionalAnimationSet walk;
    }
}