using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public Sprite mouthClosedSprite;
    public Sprite mouthOpenSprite;
    public string name;
    [TextArea(3, 10)] public string[] lines;
}