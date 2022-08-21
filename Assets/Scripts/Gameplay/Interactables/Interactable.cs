using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] bool repeatable = true;
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] UnityEvent action;

    [Inject] DialogueManager _dialogueManager;

    bool _triggered;

    public void Interact()
    {
        if (_triggered && !repeatable) return;

        _triggered = true;
        if (dialogues.Length > 0)
            _dialogueManager.StartDialogue(dialogues, InvokeAction);
        else
            InvokeAction();
    }

    void InvokeAction()
    {
        action?.Invoke();
    }
}