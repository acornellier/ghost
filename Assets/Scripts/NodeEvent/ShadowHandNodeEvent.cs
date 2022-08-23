using System.Collections;
using UnityEngine;
using Zenject;

public class ShadowHandNodeEvent : NodeEvent
{
    [SerializeField] ShadowHand shadowHand;
    [SerializeField] float prepareTime = 2;
    [SerializeField] float prepareSpeed = 5;
    [SerializeField] float chargeSpeed = 30;

    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        _ghost.StartCasting();

        shadowHand.Prepare(prepareSpeed);
        yield return new WaitForSeconds(prepareTime);
        yield return shadowHand.CO_Charge(chargeSpeed);
    }
}