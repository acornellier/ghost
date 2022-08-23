using System.Collections;
using UnityEngine;
using Zenject;

public class GhostMoveNodeEvent : NodeEvent
{
    [SerializeField] bool move = true;

    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        _ghost.Moving = move;
        yield break;
    }
}