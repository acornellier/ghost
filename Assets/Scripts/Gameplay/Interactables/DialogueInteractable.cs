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

    [SerializeField] AudioClip clip;

    [Inject] DialogueManager _dialogueManager;
    [Inject] MusicPlayer _musicPlayer;
    [Inject] SavedStateManager _savedStateManager;

    public bool disabled;

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
        if (disabled) return;

        if (_triggered && repeatDialogues.Length > 0)
        {
            _dialogueManager.StartDialogue(repeatDialogues);
            return;
        }

        if (_triggered && !repeatable)
            return;

        _triggered = true;
        _dialogueManager.StartDialogue(dialogues, DialogueCallback);

        if (savedStateKey.Length > 0)
        {
            _savedStateManager.SetBool(savedStateKey);
            _savedStateManager.Save();
        }
    }

    void DialogueCallback()
    {
        SwapSprite();
        PlaySound();
    }

    void SwapSprite()
    {
        if (!spriteSwap) return;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            spriteRenderer.sprite = spriteSwap;
    }

    void PlaySound()
    {
        if (!clip) return;

        _musicPlayer.PlayOneShot(clip);
    }
}