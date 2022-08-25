using System.Collections;
using UnityEngine;
using Zenject;

public class PrepareAllShadowHandNodeEvent : NodeEvent
{
    [SerializeField] float prepareSpeed = 5;

    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        _ghost.StartCasting();

        var shadowHands = FindObjectsOfType<ShadowHand>();
        foreach (var shadowHand in shadowHands)
        {
            shadowHand.Prepare(prepareSpeed);
        }

        yield break;
    }
}