using System.Collections;
using UnityEngine;
using Zenject;

public class SavePrefsNodeEvent : NodeEvent
{
    [SerializeField] string key = "default";
    [SerializeField] bool value = true;

    [Inject] SavedStateManager _savedStateManager;

    protected override IEnumerator CO_Run()
    {
        _savedStateManager.SavedState.bools[key] = value;
        _savedStateManager.Save();
        yield break;
    }
}