using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class DialogueInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] bool repeatable = true;
    [SerializeField] Dialogue[] dialogues;
    [SerializeField] Dialogue[] repeatDialogues;

    [SerializeField] string savedStateKey;
    [SerializeField] Sprite spriteSwap;

    [Inject] DialogueManager _dialogueManager;
    [Inject] SavedStateManager _savedStateManager;

    bool _triggered;

    void Awake()
    {
        if (savedStateKey.Length > 0)
        {
            _savedStateManager.SavedState.bools.TryGetValue(savedStateKey, out _triggered);
            if (_triggered)
                SwapSprite();
        }
    }

    public void Interact()
    {
        if (_triggered && repeatDialogues.Length > 0)
        {
            _dialogueManager.StartDialogue(repeatDialogues);
            return;
        }

        if (_triggered && !repeatable)
            return;

        _triggered = true;
        _dialogueManager.StartDialogue(dialogues, SwapSprite);

        if (savedStateKey.Length > 0)
        {
            _savedStateManager.SetBool(savedStateKey);
            _savedStateManager.Save();
        }
    }

    void SwapSprite()
    {
        if (!spriteSwap) return;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            spriteRenderer.sprite = spriteSwap;
    }
}