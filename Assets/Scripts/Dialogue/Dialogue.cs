using System;
using TMPro;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public DialogueCharacter character;
    public DialogueFontSize fontSize = DialogueFontSize.Normal;
    public FontStyles fontStyle;
    [TextArea(3, 10)] public string line;
}

public enum DialogueFontSize
{
    Small,
    Normal,
    Large,
    Huge,
}