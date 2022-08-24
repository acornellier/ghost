using System.Collections;
using UnityEngine;
using Zenject;

public class ShadowHandVerticalNodeEvent : NodeEvent
{
    [SerializeField] ShadowHandVertical shadowHand;
    [SerializeField] float prepareTime = 2;
    [SerializeField] float prepareSpeed = 5;
    [SerializeField] bool onPlayer;

    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        _ghost.StartCasting();

        shadowHand.Prepare(prepareSpeed, onPlayer);
        yield return new WaitForSeconds(prepareTime);
        yield return shadowHand.CO_Charge();
    }
}