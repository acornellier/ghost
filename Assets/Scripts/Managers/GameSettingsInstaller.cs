using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public AudioManager.Settings audioManager;
    public Bullet bulletPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(audioManager);
        Container.BindInstance(bulletPrefab);
    }
}