using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] private DialogueEntry[] dialogueEntries;
    private DisplayText displayText;
    [SerializeField] private bool stopMove;

    private void Start()
    {
        displayText = FindObjectOfType<DisplayText>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!stopMove)
            {
                displayText.stopPlayerFromMoving = false;
            }
            displayText.SetDialogue(dialogueEntries);
            displayText.playText();
        }
    }
}
