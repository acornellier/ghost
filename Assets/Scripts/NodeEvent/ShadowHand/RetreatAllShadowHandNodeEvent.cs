using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class RetreatAllShadowHandNodeEvent : NodeEvent
{
    [SerializeField] float retreatSpeed = 5;

    protected override IEnumerator CO_Run()
    {
        var shadowHands = FindObjectsOfType<ShadowHand>();
        foreach (var shadowHand in shadowHands)
        {
            StartCoroutine(shadowHand.CO_Retreat(retreatSpeed));
        }

        yield break;
    }
}