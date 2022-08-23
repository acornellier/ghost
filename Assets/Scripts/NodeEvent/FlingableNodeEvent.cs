using System.Collections;
using UnityEngine;

public class FlingableNodeEvent : NodeEvent
{
    [SerializeField] Flingable flingable;
    [SerializeField] float liftTime = 2;

    protected override IEnumerator CO_Run()
    {
        flingable.Lift();
        yield return new WaitForSeconds(liftTime);
        flingable.Fling();
    }
}