using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNpc : Interactable
{
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private string _text = "Talk";
    [SerializeField] private DisplayText displayText;
    [SerializeField] private bool hasTalkedToNPC;
    private void Start()
    {
        displayText = FindObjectOfType<DisplayText>();
    }
    public override void OnFocus()
    {
        if(!hasTalkedToNPC){
            UIInteract.Instance.ShowText("Talk");
        }
        else{
            UIInteract.Instance.ShowText(_text);
        }
        
    }

    public override void OnInteract()
    {
        displayText.stopPlayerFromMoving = true;
        displayText.SetDialogue(dialogueEntries);
        displayText.playText();
        hasTalkedToNPC = true;
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }
}
