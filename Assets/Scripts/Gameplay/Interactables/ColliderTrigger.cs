using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ColliderTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent unityEvent;
    [SerializeField] bool repeatable;

    bool _triggered;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered && !repeatable) return;

        var player = col.GetComponent<Player>();
        if (!player) return;

        _triggered = true;
        unityEvent.Invoke();
    }
}