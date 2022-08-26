using System.Collections;
using UnityEngine;
using Zenject;

public class ShadowHandNodeEvent : NodeEvent
{
    [SerializeField] ShadowHand shadowHand;
    [SerializeField] float prepareTime = 2;
    [SerializeField] float prepareSpeed = 5;
    [SerializeField] float chargeSpeed = 50;

    [Inject] Ghost _ghost;

    void OnValidate()
    {
        if (shadowHand)
        {
            var trimmedName = shadowHand.name.Replace("ShadowHand", "");
            gameObject.name = "SH_" + trimmedName;
        }
    }

    protected override IEnumerator CO_Run()
    {
        _ghost.StartCasting();

        shadowHand.Prepare(prepareSpeed);
        yield return new WaitForSeconds(prepareTime);
        yield return shadowHand.CO_Charge(chargeSpeed);
    }
}