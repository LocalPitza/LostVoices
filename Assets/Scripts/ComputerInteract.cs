using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComputerInteract : Interactable
{

    public List<GameObject> turnOn = new List<GameObject>();
    public List<GameObject> turnOff = new List<GameObject>();
    bool alreadyOn = false;

    public override void OnFocus()
    {
        UIInteract.Instance.ShowText("Redirect Power");
    }

    public override void OnInteract()
    {
        if(!alreadyOn)
        {
            foreach (var gameObject in turnOn)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(true);
                }
            }
            foreach (var gameObject in turnOff)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }
            alreadyOn = true;
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }
}
