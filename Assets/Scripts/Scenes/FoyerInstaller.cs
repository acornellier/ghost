using System.Collections;
using UnityEngine;
using Zenject;

public class FoyerInstaller : MonoBehaviour
{
    [SerializeField] GameObject combatTrigger;
    [SerializeField] GameObject[] roundTwoDeactivate;
    [SerializeField] NodeEvent combatSequence2;

    [Inject] Ghost _ghost;
    [Inject] Player _player;
    [Inject] SavedStateManager _savedStateManager;

    void Start()
    {
        var ballroomCombat = _savedStateManager.IsBoolSet("BallroomCombat");
        var halfFoyerCombat = _savedStateManager.IsBoolSet("FoyerCombatHalf");

        // TODO
        // ballroomCombat = true;

        foreach (var go in roundTwoDeactivate)
        {
            go.SetActive(!ballroomCombat);
        }

        if (halfFoyerCombat)
        {
            StartCoroutine(LoadHalfCombat());
            return;
        }

        combatTrigger.SetActive(ballroomCombat);
    }

    IEnumerator LoadHalfCombat()
    {
        if (_savedStateManager.SavedState.nextSpawn != "Outside")
        {
            _savedStateManager.SavedState.nextSpawn = "Outside";
            _savedStateManager.Save();
        }

        // delay spawn and combat by 1 tick to avoid random start orders
        yield return null;
        _player.SpawnAtSpawnPoint();
        _ghost.StartCombat(combatSequence2);
    }
}