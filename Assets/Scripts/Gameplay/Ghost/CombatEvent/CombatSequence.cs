using System.Collections;
using UnityEngine;

public class CombatSequence : CombatEvent
{
    [SerializeField] CombatEvent[] combatEvents;

    public override void Run()
    {
        StartCoroutine(CO_Run());
    }

    IEnumerator CO_Run()
    {
        foreach (var combatEvent in combatEvents)
        {
            combatEvent.Run();
            yield return new WaitUntil(() => combatEvent.IsDone);
        }
    }
}