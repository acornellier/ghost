using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Image heartPrefab;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    [Inject] PlayerHealth _playerHealth;

    readonly List<Image> _hearts = new();

    void OnEnable()
    {
        _playerHealth.onHealthChange += HandleHealthChange;
    }

    void OnDisable()
    {
        _playerHealth.onHealthChange += HandleHealthChange;
    }

    void Start()
    {
        for (var i = 0; i < _playerHealth.MaxHealth; ++i)
        {
            var heart = Instantiate(heartPrefab, transform);
            heart.sprite = fullHeart;
            _hearts.Add(heart);
        }

        HandleHealthChange(_playerHealth.MaxHealth, _playerHealth.Health);
    }

    void HandleHealthChange(float prevHealth, float curHealth)
    {
        var min = Mathf.Min(prevHealth, curHealth);
        var max = Mathf.Max(prevHealth, curHealth);

        for (var i = Mathf.FloorToInt(min); i < Mathf.CeilToInt(max); ++i)
        {
            var heartHealth = Mathf.CeilToInt(Mathf.Clamp01(curHealth - i));
            _hearts[i].sprite = heartHealth == 0 ? emptyHeart : fullHeart;
        }
    }
}