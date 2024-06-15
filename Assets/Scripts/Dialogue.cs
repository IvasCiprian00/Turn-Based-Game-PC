using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string name;
    public Sprite speaker;
    public string trigger;
    public AudioClip chatter;
    [TextArea(3, 10)]
    public string line;
}
