using System.Collections;
using UnityEngine;
using Zenject;

public class ShadowHandVerticalNodeEvent : NodeEvent
{
    [SerializeField] ShadowHandVertical shadowHand;
    [SerializeField] float prepareTime = 2;
    [SerializeField] bool onPlayer = true;

    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        _ghost.StartCasting();

        yield return shadowHand.CO_Prepare(prepareTime, onPlayer);
        yield return shadowHand.CO_Charge();
    }
}