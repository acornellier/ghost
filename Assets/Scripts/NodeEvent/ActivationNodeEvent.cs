using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ActivationNodeEvent : NodeEvent
{
    [SerializeField] GameObject go;
    [SerializeField] bool active = true;

    protected override IEnumerator CO_Run()
    {
        go.SetActive(active);
        yield break;
    }
}