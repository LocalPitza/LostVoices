using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NormalDoorScript : Interactable
{
    public GameObject Door;
    public bool isOpen;
    public bool moveSnap;
    public float moveSpeed;
    private string _text = "Open Door";
    public AudioSource _audio;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;

    public override void OnFocus()
    {
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnInteract()
    {
        if(!isOpen){
            _audio.Play();
            Door.transform.DOMove(end,moveSpeed,moveSnap);
            isOpen = true;
            _text = "Close Door";
        }
        else{
            _audio.Play();
            Door.transform.DOMove(start,moveSpeed,moveSnap);
            isOpen = false;
            _text = "Open Door";
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }

}
