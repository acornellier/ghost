using System.Collections;
using UnityEngine;

public abstract class NodeEvent : MonoBehaviour
{
    [SerializeField] float startDelayTime;
    [SerializeField] float endDelayTime;

    public bool IsDone { get; protected set; }

    public void Run()
    {
        StartCoroutine(CO_RunWrapper());
    }

    IEnumerator CO_RunWrapper()
    {
        IsDone = false;

        if (startDelayTime != 0)
            yield return new WaitForSeconds(startDelayTime);

        yield return StartCoroutine(CO_Run());

        if (endDelayTime != 0)
            yield return new WaitForSeconds(endDelayTime);

        IsDone = true;
    }

    protected abstract IEnumerator CO_Run();
}