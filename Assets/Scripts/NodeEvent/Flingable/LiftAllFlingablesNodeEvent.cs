using System.Collections;
using UnityEngine;
using Zenject;

public class LiftAllFlingablesNodeEvent : NodeEvent
{
    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        _ghost.StartCasting();

        var flingables = FindObjectsOfType<Flingable>();

        foreach (var flingable in flingables)
        {
            flingable.Lift();
        }

        yield break;
    }
}