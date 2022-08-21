using TMPro;
using UnityEngine;
using Zenject;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    [Inject] PlayerHealth _playerHealth;

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
        HandleHealthChange(_playerHealth.Health, _playerHealth.Health);
    }

    void HandleHealthChange(float prevHealth, float curHealth)
    {
        text.text = $"HP: {curHealth}/{_playerHealth.MaxHealth}";
    }
}