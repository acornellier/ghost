using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public DialogueCharacter character;
    [TextArea(3, 10)] public string line;
}