using UnityEngine;
using Zenject;

public class SavedStateActivator : MonoBehaviour
{
    [SerializeField] string key;
    [SerializeField] GameObject[] activate;
    [SerializeField] GameObject[] deactivate;

    [Inject] SavedStateManager _savedStateManager;

    void Awake()
    {
        if (!_savedStateManager.IsBoolSet(key)) return;

        foreach (var go in activate)
        {
            go.SetActive(true);
        }

        foreach (var go in deactivate)
        {
            go.SetActive(false);
        }
    }
}