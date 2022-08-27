using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] DialogueImage topImage;
    [SerializeField] DialogueImage bottomImage;

    public Action onDialogueStart;
    public Action onDialogueEnd;

    PlayerInputActions.PlayerActions _actions;

    DialogueImage _activeDialogueImage;
    Queue<Dialogue> _dialogues;
    Action _callback;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
    }

    void Start()
    {
        topImage.gameObject.SetActive(false);
        bottomImage.gameObject.SetActive(false);
        _actions.Interact.performed += OnNextInput;
    }

    void OnDisable()
    {
        StopDialogue();
        _actions.Interact.performed -= OnNextInput;
    }

    public void StartDialogue(IEnumerable<Dialogue> dialogues, Action callback = null)
    {
        onDialogueStart?.Invoke();
        _actions.Enable();

        _dialogues = new Queue<Dialogue>(dialogues);
        _callback = callback;
        TypeNextLine();
    }

    public void StopDialogue()
    {
        _actions.Disable();
        if (_activeDialogueImage)
        {
            _activeDialogueImage.StopCoroutine();
            _activeDialogueImage.gameObject.SetActive(false);
            _activeDialogueImage = null;
        }

        onDialogueEnd?.Invoke();
        _callback?.Invoke();
        _callback = null;
    }

    void OnNextInput(InputAction.CallbackContext ctx)
    {
        if (_activeDialogueImage && !_activeDialogueImage.IsDone())
        {
            _activeDialogueImage.SkipToEndOfLine();
            return;
        }

        TypeNextLine();
    }

    void TypeNextLine()
    {
        if (_dialogues.Count <= 0)
        {
            StopDialogue();
            return;
        }

        if (_activeDialogueImage)
            _activeDialogueImage.gameObject.SetActive(false);

        var nextDialogue = _dialogues.Dequeue();
        _activeDialogueImage = nextDialogue.topOfScreen ? topImage : bottomImage;
        _activeDialogueImage.gameObject.SetActive(true);
        _activeDialogueImage.TypeNextLine(nextDialogue);
    }
}