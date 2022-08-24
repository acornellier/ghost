using System.Collections;
using UnityEngine;
using Zenject;

public class GhostFlyNodeEvent : NodeEvent
{
    [SerializeField] Transform destination;
    [SerializeField] float speed = 10;
    [SerializeField] bool fly = true;
    [SerializeField] Vector2 offset;

    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        yield return StartCoroutine(
            _ghost.CO_FlyTo((Vector2)destination.position + offset, speed, fly)
        );
    }
}