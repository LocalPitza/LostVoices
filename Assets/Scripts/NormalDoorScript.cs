using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NormalDoorScript : Interactable
{
    public GameObject Door;
    public bool isOpen;
    public bool useOnce;
    public bool moveSnap;
    public float moveSpeed;
    [SerializeField] private string _text = "Interact";
    public AudioSource _audio;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    [SerializeField] private bool hasBeenUsed = false;
    public override void OnFocus()
    {
        if(!isOpen && !useOnce){
            UIInteract.Instance.ShowText(_text);
        }
        if(!isOpen && useOnce){
            UIInteract.Instance.ShowText(_text);
        }
        if(isOpen && useOnce){
            UIInteract.Instance.ShowText("Door is Open");
        }

    }

    public override void OnInteract()
    {
        if (useOnce && hasBeenUsed)
            return;

        if (!isOpen)
        {
            _audio.Play();
            Door.transform.DOMove(end, moveSpeed, moveSnap);
            isOpen = true;
        }
        else
        {
            _audio.Play();
            Door.transform.DOMove(start, moveSpeed, moveSnap);
            isOpen = false;
        }
        if (useOnce)
        {
            hasBeenUsed = true;
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }
    
}
