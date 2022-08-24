using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NodeEventTrigger : MonoBehaviour
{
    [SerializeField] NodeEvent nodeEvent;
    [SerializeField] bool repeatable = true;

    bool _triggered;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered && !repeatable) return;

        var player = col.GetComponent<Player>();
        if (!player) return;

        _triggered = true;
        nodeEvent.Run();
    }
}