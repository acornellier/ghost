using System.Collections;
using UnityEngine;
using Zenject;

public class DialogueNodeEvent : NodeEvent
{
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;
    [Inject] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        _ghost.StopAttackingCasting();
        _dialogueManager.StartDialogue(dialogues, () => IsDone = true);
        yield return new WaitUntil(() => IsDone);
    }
}