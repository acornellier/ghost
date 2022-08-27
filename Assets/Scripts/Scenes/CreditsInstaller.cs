using Zenject;

public class CreditsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
    }

    public override void Start()
    {
        SavedStateManager.UnlockHardmode();
    }
}