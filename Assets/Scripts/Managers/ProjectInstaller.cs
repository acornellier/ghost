using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SavedStateManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle();

        Container.Bind<DialogueManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelLoader>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MusicPlayer>().FromComponentInHierarchy().AsSingle();
    }
}