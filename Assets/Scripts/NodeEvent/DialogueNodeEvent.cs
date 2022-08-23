using System.Collections;
using UnityEngine;
using Zenject;

public class DialogueNodeEvent : NodeEvent
{
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;
    [InjectOptional] Ghost _ghost;

    protected override IEnumerator CO_Run()
    {
        if (_ghost)
            _ghost.StopAttackingCasting();

        _dialogueManager.StartDialogue(dialogues, () => IsDone = true);
        yield return new WaitUntil(() => IsDone);
    }
}