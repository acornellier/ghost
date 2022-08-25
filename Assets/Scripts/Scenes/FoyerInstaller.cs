using UnityEngine;
using Zenject;

public class FoyerInstaller : MonoInstaller
{
    [SerializeField] GameObject[] roundTwoActivate;
    [SerializeField] GameObject[] roundTwoDeactivate;

    [Inject] SavedStateManager _savedStateManager;

    public override void InstallBindings()
    {
    }

    public override void Start()
    {
        var ballroomCombat = _savedStateManager.IsBoolSet("BallroomCombat");
        var foyerCombat = _savedStateManager.IsBoolSet("FoyerCombat");
        var isRoundTwo = ballroomCombat && !foyerCombat;

        // TODO
        isRoundTwo = true;

        foreach (var go in roundTwoActivate)
        {
            go.SetActive(isRoundTwo);
        }

        foreach (var go in roundTwoDeactivate)
        {
            go.SetActive(!isRoundTwo);
        }
    }
}