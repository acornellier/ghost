using System.Collections;
using UnityEngine;
using Zenject;

public class FoyerInstaller : MonoBehaviour
{
    [SerializeField] GameObject combatTrigger;
    [SerializeField] GameObject[] roundTwoDeactivate;
    [SerializeField] NodeEvent combatSequence2;
    [SerializeField] Transform playerSpawnHalf;

    [Inject] Ghost _ghost;
    [Inject] Player _player;
    [Inject] SavedStateManager _savedStateManager;

    void Start()
    {
        var ballroomCombat = _savedStateManager.IsBoolSet("BallroomCombat");
        var foyerCombat = _savedStateManager.IsBoolSet("FoyerCombat");
        var isRoundTwo = ballroomCombat && !foyerCombat;
        var halfFoyerCombat = _savedStateManager.IsBoolSet("FoyerCombatHalf");

        // TODO
        // isRoundTwo = true;

        foreach (var go in roundTwoDeactivate)
        {
            go.SetActive(!isRoundTwo);
        }

        if (halfFoyerCombat && !foyerCombat)
        {
            StartCoroutine(LoadHalfCombat());
            return;
        }

        combatTrigger.SetActive(isRoundTwo);
    }

    IEnumerator LoadHalfCombat()
    {
        yield return null;
        _player.transform.position = playerSpawnHalf.position;
        _ghost.StartCombat(combatSequence2);
    }
}