using System.Collections;
using UnityEngine;
using Zenject;

public class SettleAllFlingablesNodeEvent : NodeEvent
{
    protected override IEnumerator CO_Run()
    {
        var flingables = FindObjectsOfType<Flingable>();

        foreach (var flingable in flingables)
        {
            flingable.Drop();
        }

        yield break;
    }
}