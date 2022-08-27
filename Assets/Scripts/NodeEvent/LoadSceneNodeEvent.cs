using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Zenject;

public class LoadSceneNodeEvent : NodeEvent
{
    [SerializeField] string scene;
    [SerializeField] bool useLevelLoader = true;

    [Inject] LevelLoader _levelLoader;

    protected override IEnumerator CO_Run()
    {
        if (useLevelLoader)
            _levelLoader.LoadScene(scene);
        else
            SceneManager.LoadScene(scene);
        yield break;
    }
}