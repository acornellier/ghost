using System.Collections;
using UnityEngine;

public abstract class NodeEvent : MonoBehaviour
{
    [SerializeField] float startDelayTime;
    [SerializeField] float endDelayTime;

    [ReadOnly] [SerializeField] bool isDone;
    [ReadOnly] [SerializeField] protected bool isActive;

    public bool IsDone
    {
        get => isDone;
        protected set => isDone = value;
    }

    public void Run()
    {
        StartCoroutine(CO_RunWrapper());
    }

    IEnumerator CO_RunWrapper()
    {
        isActive = true;
        isDone = false;

        if (startDelayTime != 0)
            yield return new WaitForSeconds(startDelayTime);

        yield return StartCoroutine(CO_Run());

        if (endDelayTime != 0)
            yield return new WaitForSeconds(endDelayTime);

        isActive = false;
        isDone = true;
    }

    protected abstract IEnumerator CO_Run();
}