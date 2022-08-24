﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueImage : MonoBehaviour
{
    [SerializeField] Image talkingHead;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text contents;
    [SerializeField] float textSpeed = 50;
    [SerializeField] float spriteSpeed = 10;
    [SerializeField] float timeBetweenSentences = 0.5f;

    public Action onDialogueStart;
    public Action onDialogueEnd;

    Dialogue _currentDialogue;
    Coroutine _coroutine;
    Action _callback;

    void OnNextInput()
    {
        if (contents.maxVisibleCharacters >= _currentDialogue.line.Length)
        {
            TypeNextLine();
            return;
        }

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        contents.maxVisibleCharacters = _currentDialogue.line.Length;
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

    IEnumerator CO_TypeNextLine(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
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
            while (charIndex < sentence.Length &&
                   contents.maxVisibleCharacters < _currentDialogue.line.Length)
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