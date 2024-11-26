using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
public class DisplayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private bool IsDisplaying = false;
    [SerializeField] public bool stopPlayerFromMoving = true;

    [SerializeField] private DialogueEntry[] customDialogue;
    

    private void Start()
    {
        DialogueText = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<TextMeshProUGUI>();
    }

    public void SetDialogue(DialogueEntry[] dialogueEntries)
    {
        customDialogue = dialogueEntries;
    }

    public void playText()
    {
        if (!IsDisplaying)
        {
            IsDisplaying = true;
            if(stopPlayerFromMoving)
            {
                FirstPersonController.instance.CanMove = false;
            }         
            StartCoroutine(DialogueDisplay(customDialogue));
        }
    }

    private IEnumerator DialogueDisplay(DialogueEntry[] DialogueEntries)
    {
        for (int i = 0; i < DialogueEntries.Length; i++)
        {
            if (!DialogueEntries[i].canRepeat && DialogueEntries[i].shown)
            {
                continue;
            }

            StringSplitter(DialogueEntries[i]);
            yield return new WaitForSeconds((DialogueEntries[i].text.Length + 45) * 0.035f);
            
            DialogueEntries[i].shown = true;
        }
        
        clearDialogue();
        FirstPersonController.instance.CanMove = true;
        IsDisplaying = false;
    }

    private void StringSplitter(DialogueEntry entry)
    {
        DialogueText.text = "";
        string[] Characters = new string[entry.text.Length];

        for (int i = 0; i < entry.text.Length; i++)
        {
            Characters[i] = entry.text[i].ToString();
        }
        StartCoroutine(StringDisplayDelay(Characters, entry.speed));
    }

    private IEnumerator StringDisplayDelay(string[] Characters, float speed)
    {
        var soundManager = FindObjectOfType<SoundManager>();
        
        for (int i = 0; i < Characters.Length - 1; i++)
        {
            DialogueText.text += Characters[i];

            if (soundManager != null)
            {
                PlayRandomSound(Characters, i, soundManager);
            }

            yield return new WaitForSecondsRealtime(speed);
        }
    }

    private void PlayRandomSound(string[] Characters, int currentIndex, SoundManager soundManager)
    {
        if (Characters[Characters.Length - 1] == "R")
        {
            if (currentIndex % 6 == 0) soundManager.Play("DeepCharacterSpeak");
            if (currentIndex % 3 == 0)
            {
                int randomIndex = Random.Range(1, 3);
                soundManager.Play($"Speak{randomIndex}");
            }
        }
        else if (Characters[Characters.Length - 1] == "L")
        {
            if (currentIndex % 6 == 0) soundManager.Play("CharacterSpeak");
            if (currentIndex % 3 == 0)
            {
                int randomIndex = Random.Range(3, 5);
                soundManager.Play($"Speak{randomIndex}");
            }
        }
        else if (Characters[Characters.Length - 1] == "P")
        {
            if (currentIndex % 6 == 0) soundManager.Play("LightCharacterSpeak");
            if (currentIndex % 3 == 0)
            {
                int randomIndex = Random.Range(1, 3);
                soundManager.Play($"Speak{randomIndex}");
            }
        }
    }

    public void clearDialogue()
    {
        DialogueText.text = "";
        customDialogue = new DialogueEntry[0];
    }
}
