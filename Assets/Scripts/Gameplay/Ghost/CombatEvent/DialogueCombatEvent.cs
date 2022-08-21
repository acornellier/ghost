using UnityEngine;
using Zenject;

public class DialogueCombatEvent : CombatEvent
{
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;

    public override void Run()
    {
        _dialogueManager.StartDialogue(dialogues, () => IsDone = true);
    }
}