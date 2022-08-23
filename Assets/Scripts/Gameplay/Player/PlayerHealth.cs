using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] float immuneTime = 0.5f;

    public int MaxHealth => maxHealth;

    float _health;

    Coroutine _immuneCoroutine;

    public float Health
    {
        get => _health;
        set
        {
            if (_immune && value < _health) return;

            var prevHealth = _health;
            _health = Mathf.Clamp(value, 0, MaxHealth);

            if (Math.Abs(_health - prevHealth) < 0.01f)
                return;

            if (_health < prevHealth)
                StartCoroutine(CO_TemporaryImmune());

            onHealthChange?.Invoke(prevHealth, _health);
        }
    }

    public Action<float, float> onHealthChange;

    bool _immune;

    public bool Immune
    {
        get => _immune;
        set
        {
            _immune = value;

            if (_immuneCoroutine != null)
                StopCoroutine(_immuneCoroutine);
        }
    }

    void Awake()
    {
        _health = maxHealth;
    }

    IEnumerator CO_TemporaryImmune()
    {
        if (_immuneCoroutine != null)
            StopCoroutine(_immuneCoroutine);

        _immune = true;
        yield return new WaitForSeconds(immuneTime);
        _immune = false;
    }
}