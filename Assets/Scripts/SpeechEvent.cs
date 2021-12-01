using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeechEvent
{
    public string text;
    public float displayTime;
    public SpeechController2D.Mode mode;

    public SpeechEvent(string text, float displayTime, SpeechController2D.Mode mode)
    {
        this.text = text;
        this.displayTime = displayTime;
        this.mode = mode;
    }
}
