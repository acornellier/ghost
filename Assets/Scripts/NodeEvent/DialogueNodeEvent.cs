using System.Collections;
using UnityEngine;
using Zenject;

public class DialogueNodeEvent : NodeEvent
{
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;

    protected override IEnumerator CO_Run()
    {
        _dialogueManager.StartDialogue(dialogues, () => IsDone = true);
        yield return new WaitUntil(() => IsDone);
    }
}