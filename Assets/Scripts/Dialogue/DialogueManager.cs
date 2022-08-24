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

    public Action onDialogueStart;
    public Action onDialogueEnd;

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
        wrapper.SetActive(false);
        _initialFontStyle = contents.fontStyle;
        _actions.Interact.performed += OnNextInput;
        _actions.SkipDialogue.performed += OnSkipDialogue;
    }

    void OnDisable()
    {
        _actions.Interact.performed -= OnNextInput;
        _actions.SkipDialogue.performed -= OnSkipDialogue;
    }

    public void StartDialogue(IEnumerable<Dialogue> dialogues, Action callback = null)
    {
        onDialogueStart?.Invoke();
        _actions.Enable();

        wrapper.SetActive(true);
        _dialogues = new Queue<Dialogue>(dialogues);
        _callback = callback;
        TypeNextLine();
    }

    public void StopDialogue()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _actions.Disable();

        wrapper.SetActive(false);
        onDialogueEnd?.Invoke();
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

    void OnSkipDialogue(InputAction.CallbackContext ctx)
    {
        StopDialogue();
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
        talkingHead.sprite = _currentDialogue.character.mouthClosedSprite;
        title.text = _currentDialogue.character.characterName;
        InitializeContents(_currentDialogue);

        if (_currentDialogue.wobble != Wobble.None)
        {
            contents.maxVisibleCharacters = _currentDialogue.line.Length;
            while (true)
            {
                WobbleContents();
                yield return null;
            }
        }

        var sentences = SplitIntoSentences(_currentDialogue.line);
        contents.maxVisibleCharacters = 0;
        foreach (var sentence in sentences)
        {
            var t = 0f;
            var charIndex = 0;
            while (charIndex < sentence.Length)
            {
                t += Time.deltaTime;

                var newCharIndex = Mathf.Clamp(Mathf.CeilToInt(t * textSpeed), 0, sentence.Length);
                contents.maxVisibleCharacters += newCharIndex - charIndex;
                charIndex = newCharIndex;

                WobbleContents();

                talkingHead.sprite = Mathf.Floor(t * spriteSpeed) % 2 == 0
                    ? _currentDialogue.character.mouthClosedSprite
                    : _currentDialogue.character.mouthOpenSprite;

                yield return null;
            }

            talkingHead.sprite = _currentDialogue.character.mouthClosedSprite;
            yield return new WaitForSeconds(timeBetweenSentences);
        }
    }

    void WobbleContents()
    {
        if (_currentDialogue.wobble == Wobble.None) return;

        contents.ForceMeshUpdate();
        var mesh = contents.mesh;
        var vertices = mesh.vertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            var offset = Time.time + i;
            vertices[i] += new Vector3(Mathf.Sin(offset * 50f), Mathf.Cos(offset * 25f));
        }

        mesh.vertices = vertices;
        contents.canvasRenderer.SetMesh(mesh);
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
        contents.text = _currentDialogue.line;
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