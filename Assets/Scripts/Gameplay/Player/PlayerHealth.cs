using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] float immuneTime = 0.5f;

    public int MaxHealth => maxHealth;

    float _health;

    public float Health
    {
        get => _health;
        set
        {
            if (immune && value < _health) return;

            var prevHealth = _health;
            _health = value;
            onHealthChange?.Invoke(prevHealth, _health);

            if (_health < prevHealth)
                StartCoroutine(Immune());
        }
    }

    public Action<float, float> onHealthChange;

    [NonSerialized] public bool immune;

    void Awake()
    {
        Health = maxHealth;
    }

    IEnumerator Immune()
    {
        immune = true;
        yield return new WaitForSeconds(immuneTime);
        immune = false;
    }
}