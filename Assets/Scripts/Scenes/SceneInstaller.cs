using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] AudioClip overrideMusic;

    [Inject] LevelLoader _levelLoader;
    [Inject] DialogueManager _dialogueManager;
    [Inject] MusicPlayer _musicPlayer;

    public override void InstallBindings()
    {
        Container.Bind<MonoPool<Bullet>>().AsSingle();

        Container.Bind<HealthDisplay>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Ghost>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
    }

    public override void Start()
    {
        base.Start();
        _levelLoader.StartScene();
        _musicPlayer.PlayMusic(overrideMusic ? overrideMusic : _musicPlayer.defaultMusic);
    }

    void OnDisable()
    {
        if (_dialogueManager)
            _dialogueManager.StopDialogue();
    }
}