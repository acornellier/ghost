using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle();

        Container.Bind<LevelLoader>().FromComponentInHierarchy().AsSingle();
    }
}