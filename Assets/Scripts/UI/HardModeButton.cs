using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class HardModeButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_Text text;

    [Inject] SavedStateManager _savedStateManager;

    PlayerInputActions _playerInputActions;

    void Awake()
    {
        _playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        _playerInputActions.Player.AllowHardMode.Enable();
        _playerInputActions.Player.AllowHardMode.performed += OnUnlockHardMode;
        button.gameObject.SetActive(SavedStateManager.IsHardModeUnlocked());
    }

    void OnDisable()
    {
        _playerInputActions.Player.AllowHardMode.Disable();
    }

    void OnUnlockHardMode(InputAction.CallbackContext _)
    {
        SavedStateManager.UnlockHardmode();
        button.gameObject.SetActive(SavedStateManager.IsHardModeUnlocked());
    }

    void Start()
    {
        UpdateText();
    }

    public void ToggleHardMode()
    {
        _savedStateManager.ToggleHardMode();
        UpdateText();
    }

    void UpdateText()
    {
        text.text = SavedStateManager.IsHardMode ? "Turn Off Hard Mode" : "Turn On Hard Mode";
    }
}