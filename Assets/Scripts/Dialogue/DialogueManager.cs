using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] Image talkingHead;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text contents;
    [SerializeField] float textSpeed = 50;
    [SerializeField] float spriteSpeed = 10;
    [SerializeField] float timeBetweenSentences = 0.5f;

    [Inject] Player _player;

    PlayerInputActions.PlayerActions _actions;

    Queue<Dialogue> _dialogues;
    Dialogue _currentDialogue;
    Coroutine _coroutine;
    Action _callback;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
    }

    void Start()
    {
        _actions.Interact.performed += OnNextInput;
    }

    void OnDisable()
    {
        _actions.Interact.performed -= OnNextInput;
    }

    public void StartDialogue(IEnumerable<Dialogue> dialogues, Action callback = null)
    {
        _player.DisablePlayerControls();
        _actions.Enable();

        wrapper.SetActive(true);
        _dialogues = new Queue<Dialogue>(dialogues);
        _callback = callback;
        TypeNextLine();
    }

    void StopDialogue()
    {
        _actions.Disable();
        _player.EnableControls();

        wrapper.SetActive(false);
        _callback?.Invoke();
    }

    void OnNextInput(InputAction.CallbackContext ctx)
    {
        if (contents.text == _currentDialogue.line)
        {
            TypeNextLine();
            return;
        }

        StopCoroutine(_coroutine);
        contents.text = _currentDialogue.line;
    }

    void TypeNextLine()
    {
        if (_dialogues.Count <= 0)
        {
            StopDialogue();
            return;
        }

        _coroutine = StartCoroutine(CO_TypeNextLine());
    }

    IEnumerator CO_TypeNextLine()
    {
        _currentDialogue = _dialogues.Dequeue();
        title.text = _currentDialogue.character.characterName;
        contents.text = "";

        var sentences = SplitIntoSentences(_currentDialogue.line);
        foreach (var sentence in sentences)
        {
            var t = 0f;
            var charIndex = 0;
            while (charIndex < sentence.Length)
            {
                t += Time.deltaTime;

                var newCharIndex = Mathf.Clamp(Mathf.CeilToInt(t * textSpeed), 0, sentence.Length);
                contents.text += sentence[charIndex..newCharIndex];
                charIndex = newCharIndex;

                talkingHead.sprite = Mathf.Floor(t * spriteSpeed) % 2 == 0
                    ? _currentDialogue.character.mouthClosedSprite
                    : _currentDialogue.character.mouthOpenSprite;

                yield return null;
            }

            talkingHead.sprite = _currentDialogue.character.mouthClosedSprite;
            yield return new WaitForSeconds(timeBetweenSentences);
        }
    }

    static List<string> SplitIntoSentences(string line)
    {
        List<string> sentences = new();
        var sentence = "";
        foreach (var character in line)
        {
            sentence += character;
            if (character is '.' or '!' or '?')
            {
                sentences.Add(sentence);
                sentence = "";
            }
        }

        return sentences;
    }
}