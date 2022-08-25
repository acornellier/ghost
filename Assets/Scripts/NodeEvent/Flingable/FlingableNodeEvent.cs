using System.Collections;
using UnityEngine;
using Zenject;

public class FlingableNodeEvent : NodeEvent
{
    [SerializeField] Flingable flingable;
    [SerializeField] float liftTime = 2;

    [Inject] Ghost _ghost;

    void OnValidate()
    {
        if (flingable)
            gameObject.name = "Fling" + flingable.name;
    }

    protected override IEnumerator CO_Run()
    {
        _ghost.StartCasting();

        flingable.Lift();
        yield return new WaitForSeconds(liftTime);
        flingable.Fling();
    }
}