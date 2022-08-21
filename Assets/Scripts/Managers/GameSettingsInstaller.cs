using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public AudioManager.Settings audioManager;

    public override void InstallBindings()
    {
        Container.BindInstance(audioManager);
    }
}
