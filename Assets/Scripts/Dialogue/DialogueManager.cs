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
    [Inject] PlayerHealth _playerHealth;

    PlayerInputActions.PlayerActions _actions;
    FontStyles _initialFontStyle;

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
        _initialFontStyle = contents.fontStyle;
        _actions.Interact.performed += OnNextInput;
    }

    void OnDisable()
    {
        _actions.Interact.performed -= OnNextInput;
    }

    public void StartDialogue(IEnumerable<Dialogue> dialogues, Action callback = null)
    {
        _player.DisablePlayerControls();
        _playerHealth.Immune = true;
        _actions.Enable();

        wrapper.SetActive(true);
        _dialogues = new Queue<Dialogue>(dialogues);
        _callback = callback;
        TypeNextLine();
    }

    void StopDialogue()
    {
        _player.EnableControls();
        _playerHealth.Immune = false;
        _actions.Disable();

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

        if (_coroutine != null)
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
        InitializeContents(_currentDialogue);

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

    void InitializeContents(Dialogue dialogue)
    {
        contents.fontSize = dialogue.fontSize switch
        {
            DialogueFontSize.Small => 12,
            DialogueFontSize.Normal => 16,
            DialogueFontSize.Large => 24,
            DialogueFontSize.Huge => 32,
            _ => throw new ArgumentOutOfRangeException(),
        };

        contents.fontStyle = dialogue.fontStyle;
        contents.text = "";
    }

    static List<string> SplitIntoSentences(string line)
    {
        List<string> sentences = new();
        var sentence = "";
        foreach (var character in line)
        {
            if (character is not ('.' or '!' or '?'))
            {
                sentence += character;
                continue;
            }

            // account for consecutive puncutation marks
            if (sentence.Length == 0 && sentences.Count > 0)
            {
                sentences[^1] += character;
            }
            else
            {
                sentence += character;
                sentences.Add(sentence);
                sentence = "";
            }
        }

        if (sentence.Length > 0)
            sentences.Add(sentence);

        return sentences;
    }
}