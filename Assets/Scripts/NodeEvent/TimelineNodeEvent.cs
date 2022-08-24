using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineNodeEvent : NodeEvent
{
    [SerializeField] PlayableDirector timeline;

    void Update()
    {
        // TODO: DELETE BEFORE PUBLISHING
        if (Input.GetKeyDown(KeyCode.X) ||
            (Input.GetKeyDown(KeyCode.F) && timeline.state == PlayState.Playing))
        {
            timeline.time = timeline.playableAsset.duration;
            timeline.Evaluate();
            timeline.Stop();
        }
    }

    protected override IEnumerator CO_Run()
    {
        timeline.Play();
        yield return new WaitUntil(() => timeline.time >= timeline.playableAsset.duration - 0.1f);
    }
}