using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Image heartPrefab;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite emptyHeart;

    readonly List<Image> _hearts = new();

    public void Initialize(float health, float maxHealth)
    {
        for (var i = 0; i < maxHealth; ++i)
        {
            var heart = Instantiate(heartPrefab, transform);
            heart.enabled = false;
            heart.sprite = fullHeart;
            _hearts.Add(heart);
        }

        HandleHealthChange(maxHealth, health);
    }

    public void HandleHealthChange(float prevHealth, float curHealth)
    {
        var max = Mathf.Max(prevHealth, curHealth);

        for (var heartIndex = 0; heartIndex < max; ++heartIndex)
        {
            var heartHealth = Mathf.Clamp01(curHealth - heartIndex);
            _hearts[heartIndex].sprite =
                heartHealth switch
                {
                    0 => emptyHeart,
                    1 => fullHeart,
                    _ => halfHeart,
                };
        }
    }

    public void Show()
    {
        foreach (var heart in _hearts)
        {
            heart.enabled = true;
        }
    }

    public void Hide()
    {
        foreach (var heart in _hearts)
        {
            heart.enabled = false;
        }
    }
}