using System;
using TMPro;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public DialogueCharacter character;
    public DialogueFontSize fontSize = DialogueFontSize.Normal;
    public FontStyles fontStyle;
    public Wobble wobble = Wobble.None;
    [TextArea(3, 10)] public string line;
}

public enum DialogueFontSize
{
    Small,
    Normal,
    Large,
    Huge,
}

public enum Wobble
{
    None,
    Serious,
}