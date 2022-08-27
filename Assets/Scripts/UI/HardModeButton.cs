using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HardModeButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_Text text;

    [Inject] SavedStateManager _savedStateManager;

    void OnEnable()
    {
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