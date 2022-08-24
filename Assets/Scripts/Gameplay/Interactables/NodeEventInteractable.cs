using UnityEngine;

public class NodeEventInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] bool repeatable = true;
    [SerializeField] NodeEvent nodeEvent;

    bool _triggered;

    public void Interact()
    {
        if (_triggered && !repeatable) return;

        _triggered = true;
        nodeEvent.Run();
    }
}