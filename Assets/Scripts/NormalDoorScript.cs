using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NormalDoorScript : Interactable
{
    public GameObject Door;
    public bool isOpen;
    public GameObject UI;
    private string _text = "Open Door";
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;

    public override void OnFocus()
    {
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnInteract()
    {
        if(!isOpen){
            Door.transform.DOMove(end,1f,false);
            isOpen = true;
            _text = "Close Door";
        }
        else{
            Door.transform.DOMove(start,1f,false);
            isOpen = false;
            _text = "Open Door";
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }

}
