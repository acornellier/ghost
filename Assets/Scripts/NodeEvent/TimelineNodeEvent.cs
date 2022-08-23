using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineNodeEvent : NodeEvent
{
    [SerializeField] PlayableDirector timeline;

    protected override IEnumerator CO_Run()
    {
        timeline.Play();
        yield return new WaitUntil(() => timeline.state != PlayState.Playing);
    }
}