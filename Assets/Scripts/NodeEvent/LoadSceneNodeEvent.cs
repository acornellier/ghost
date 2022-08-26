using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class LoadSceneNodeEvent : NodeEvent
{
    [SerializeField] string scene;

    [Inject] LevelLoader _levelLoader;

    protected override IEnumerator CO_Run()
    {
        _levelLoader.LoadScene(scene);
        yield break;
    }
}