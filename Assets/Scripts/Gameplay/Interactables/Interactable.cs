using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] bool repeatable = true;
    [SerializeField] NodeEvent nodeEvent;

    bool _triggered;

    void OnValidate()
    {
        nodeEvent = gameObject.GetComponentInDirectChildren<NodeEvent>();
    }

    public void Interact()
    {
        if (_triggered && !repeatable) return;

        _triggered = true;
        nodeEvent.Run();
    }
}