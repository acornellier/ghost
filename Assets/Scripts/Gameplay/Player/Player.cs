using System;
using Animancer;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] Stats stats;

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
    }

    void OnDisable()
    {
        DisablePlayerControls();
    }

    void Start()
    {
        _actions.Interact.performed += (_) => Interact();
        _actions.Dash.performed += (_) => Dash();
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
        print("Interact");
    }

    void Dash()
    {
        _dashInput = true;
    }

    [Serializable]
    class Stats
    {
        public float moveSpeed;
        public float dashDistance;
    }

    [Serializable]
    class Animations
    {
    }
}