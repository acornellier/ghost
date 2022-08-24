using Zenject;

public class SceneInstaller : MonoInstaller
{
    [Inject] LevelLoader _levelLoader;
    [Inject] DialogueManager _dialogueManager;

    public override void InstallBindings()
    {
        Container.Bind<Ghost>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
    }

    public override void Start()
    {
        base.Start();
        _levelLoader.StartScene();
    }

    void OnDisable()
    {
        if (_dialogueManager)
            _dialogueManager.StopDialogue();
    }
}