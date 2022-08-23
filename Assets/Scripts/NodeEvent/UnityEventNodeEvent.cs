using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventNodeEvent : NodeEvent
{
    [SerializeField] UnityEvent unityEvent;

    protected override IEnumerator CO_Run()
    {
        unityEvent.Invoke();
        yield break;
    }
}