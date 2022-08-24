using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Image heartPrefab;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    readonly List<Image> _hearts = new();

    public void Initialize(float health, float maxHealth)
    {
        for (var i = _hearts.Count; i < maxHealth; ++i)
        {
            var heart = Instantiate(heartPrefab, transform);
            heart.sprite = fullHeart;
            _hearts.Add(heart);
        }

        HandleHealthChange(maxHealth, health);
    }

    public void HandleHealthChange(float prevHealth, float curHealth)
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