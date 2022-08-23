using UnityEngine;
using Zenject;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] bool repeatable = true;
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;

    bool _triggered;

    public void Interact()
    {
        if (_triggered && !repeatable) return;

        _triggered = true;
        _dialogueManager.StartDialogue(dialogues);
    }
}