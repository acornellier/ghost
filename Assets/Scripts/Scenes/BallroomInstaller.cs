using UnityEngine;
using Zenject;

public class BallroomInstaller : MonoInstaller
{
    [SerializeField] GameObject combatTrigger;

    [Inject] SavedStateManager _savedStateManager;

    public override void InstallBindings()
    {
    }

    public override void Start()
    {
        var atticCombat = _savedStateManager.IsBoolSet("AtticCombat");
        var ballroomCombat = _savedStateManager.IsBoolSet("BallroomCombat");

        if (atticCombat && !ballroomCombat)
            SetupSecondVersion();
        else
            combatTrigger.SetActive(false);
    }

    void SetupSecondVersion()
    {
        combatTrigger.SetActive(true);
    }
}