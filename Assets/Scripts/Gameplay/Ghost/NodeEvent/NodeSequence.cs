using System.Collections;
using UnityEngine;

public class NodeSequence : NodeEvent
{
    [SerializeField] NodeEvent[] nodeEvents;

    bool _debugSkip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            _debugSkip = true;
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