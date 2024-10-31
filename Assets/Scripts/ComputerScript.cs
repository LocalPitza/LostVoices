using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ComputerScript : Interactable
{
    public GameObject canvasToTurnOff;
    public GameObject canvasToTurnOn;
    public bool computerOn;
    public CanvasGroup fade;
    public float _fadeDuration;

    public override void OnFocus()
    {
        UIInteract.Instance.ShowText("Use Computer");
    }

    public override void OnInteract()
    {
        if(!computerOn)
        {
            switchScreen();
        }
    }

    public override void OnLoseFocus()
    {
            UIInteract.Instance.HideText();
    }

    public void switchScreen()
    {
        FirstPersonController.instance.CanMove = false;
        fade.DOFade(1, _fadeDuration).OnComplete(() =>
        {         
            fade.DOFade(0, _fadeDuration).OnComplete(() =>
            {
                canvasToTurnOff.SetActive(false);
                canvasToTurnOn.SetActive(true);
                computerOn = true;
            });
        });
    }

    public void turnOffComputer() 
    {
        if(computerOn)
        {
            fade.DOFade(1, _fadeDuration).OnComplete(() =>
            {
                fade.DOFade(0, _fadeDuration).OnComplete(() =>
                {
                    FirstPersonController.instance.CanMove = false;
                    canvasToTurnOff.SetActive(true);
                    canvasToTurnOn.SetActive(false);
                    computerOn = false;
                });
            });
        }
    }
}
