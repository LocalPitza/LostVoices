using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNpc : Interactable
{
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private string _text = "Talk";
    [SerializeField] private DisplayText displayText;
    [SerializeField] private bool hasTalkedToNPC;
    [SerializeField] private bool talkMove = true;
    private void Start()
    {
        displayText = FindObjectOfType<DisplayText>();
    }
    public override void OnFocus()
    {
        if(!hasTalkedToNPC){
            UIInteract.Instance.ShowText(_text);
        }
        else{
            UIInteract.Instance.ShowText(_text);
        }
        
    }

    public override void OnInteract()
    {
        if(!talkMove)
        {
            displayText.stopPlayerFromMoving = false;
            displayNPCText();
        }
        else
        {
            displayText.stopPlayerFromMoving = true;
            displayNPCText();
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }

    private void displayNPCText()
    {
        displayText.SetDialogue(dialogueEntries);
        displayText.playText();
        hasTalkedToNPC = true;
    }
}
