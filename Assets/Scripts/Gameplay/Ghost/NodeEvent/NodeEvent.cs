using System.Collections;
using UnityEngine;

public abstract class NodeEvent : MonoBehaviour
{
    [ReadOnly] [SerializeField] protected bool isActive;
    [ReadOnly] [SerializeField] bool isDone;

    public bool IsDone
    {
        get => isDone;
        protected set => isDone = value;
    }

    public Coroutine Run()
    {
        return StartCoroutine(CO_RunWrapper());
    }

    IEnumerator CO_RunWrapper()
    {
        isActive = true;
        isDone = false;
        yield return StartCoroutine(CO_Run());
        isActive = false;
        isDone = true;
    }

    protected abstract IEnumerator CO_Run();
}