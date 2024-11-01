using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class DialogueEntry
{
    public string text; // The dialogue text
    public float speed;
    public bool canRepeat;
    [HideInInspector] public bool shown = false;
}
