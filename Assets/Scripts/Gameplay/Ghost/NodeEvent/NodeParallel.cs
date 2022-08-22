using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeParallel : NodeEvent
{
    [SerializeField] List<NodeEvent> nodeEvents;
    [SerializeField] float timeBetweenStarts;

    // bool _debugSkip;

    void Awake()
    {
        OnValidate();
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.F))
    //         _debugSkip = true;
    // }

    void OnValidate()
    {
        nodeEvents = gameObject.GetComponentsInDirectChildren<NodeEvent>();
    }

    protected override IEnumerator CO_Run()
    {
        foreach (var nodeEvent in nodeEvents)
        {
            nodeEvent.Run();
            yield return new WaitForSeconds(timeBetweenStarts);
        }

        foreach (var nodeEvent in nodeEvents)
        {
            yield return new WaitUntil(() => nodeEvent.IsDone);
        }
    }
}