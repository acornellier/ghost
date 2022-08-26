using System;
using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerAudio playerAudio;
    [SerializeField] PlayerHealth health;
    [SerializeField] Interactors interactors;
    [SerializeField] Stats stats;
    [SerializeField] Animations animations;

    [Inject] DialogueManager _dialogueManager;
    [Inject] LevelLoader _levelLoader;
    [Inject] SavedStateManager _savedStateManager;

    PlayerInputActions.PlayerActions _actions;
    AnimancerComponent _animancer;
    Collider2D _collider;
    Rigidbody2D _body;
    SpriteRenderer _renderer;

    bool _controlsDisabledForDialogue;
    bool _animationsDisabled;
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
        _dialogueManager.onDialogueStart += HandleDialogueStart;
        _dialogueManager.onDialogueEnd += HandleDialogueEnd;
        _actions.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        DisableControls();
        health.onHealthChange -= HandleHealthChange;
        _dialogueManager.onDialogueStart -= HandleDialogueStart;
        _dialogueManager.onDialogueEnd -= HandleDialogueEnd;
        _actions.Interact.performed -= OnInteract;
    }

    void Start()
    {
        SpawnAtSpawnPoint();
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateDirection();
        UpdateAnimations();
    }

    public void SpawnAtSpawnPoint()
    {
        var nextSpawn = _savedStateManager.SavedState.nextSpawn;
        if (nextSpawn == null) return;

        foreach (var spawnPoint in FindObjectsOfType<SpawnPoint>())
        {
            if (spawnPoint.spawnTag != nextSpawn) continue;

            transform.position = spawnPoint.transform.position;
            _facingDirection = spawnPoint.spawnDirection;
            break;
        }
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
        if (_animationsDisabled) return;

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
        _controlsDisabledForDialogue = false;
    }

    public void DisableControls()
    {
        _actions.Disable();
    }

    public void EnableAnimations()
    {
        _animationsDisabled = false;
    }

    public void DisableAnimations()
    {
        _animationsDisabled = true;
    }

    void OnInteract(InputAction.CallbackContext _)
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
        playerAudio.DamageTaken();

        if (newHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(FlashSprite());
    }

    void HandleDialogueStart()
    {
        health.Immune = true;

        if (_actions.enabled)
        {
            DisableControls();
            _controlsDisabledForDialogue = true;
        }
    }

    void HandleDialogueEnd()
    {
        health.Immune = false;

        if (_controlsDisabledForDialogue)
        {
            EnableControls();
            _controlsDisabledForDialogue = false;
        }
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
        _levelLoader.ReloadScene();
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