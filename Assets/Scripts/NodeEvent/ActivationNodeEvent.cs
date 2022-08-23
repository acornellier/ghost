using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ActivationNodeEvent : NodeEvent
{
    [SerializeField] GameObject go;
    [SerializeField] GameObject[] gameObjects;
    [SerializeField] bool active = true;

    protected override IEnumerator CO_Run()
    {
        go.SetActive(active);
        foreach (var obj in gameObjects)
        {
            obj.SetActive(active);
        }

        yield break;
    }
}