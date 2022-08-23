using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSequence : NodeEvent
{
    [SerializeField] List<NodeEvent> nodeEvents;
    [SerializeField] bool playOnStart;

    bool _debugSkip;

    void Awake()
    {
        OnValidate();
    }

    void Start()
    {
        if (playOnStart) Run();
    }

    void Update()
    {
        // TODO: DELETE BEFORE PUBLISHING
        if (Input.GetKeyDown(KeyCode.F))
            _debugSkip = true;
    }

    void OnValidate()
    {
        nodeEvents = gameObject.GetComponentsInDirectChildren<NodeEvent>();
    }

    protected override IEnumerator CO_Run()
    {
        foreach (var nodeEvent in nodeEvents)
        {
            nodeEvent.Run();
            yield return new WaitUntil(
                () =>
                {
                    if (_debugSkip)
                    {
                        _debugSkip = false;
                        nodeEvent.gameObject.SetActive(false);
                        return true;
                    }

                    return nodeEvent.IsDone;
                }
            );
        }
    }
}