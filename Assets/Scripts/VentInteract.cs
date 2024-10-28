using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VentInteract : Interactable
{
    public Transform teleportTarget;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;

    [SerializeField] private Transform player;
    [SerializeField] private bool isTeleporting = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0;
        }
    }

    public override void OnFocus()
    {
            UIInteract.Instance.ShowText("Use Vent");
    }

    public override void OnInteract()
    {
        if (!isTeleporting)
        {
            StartTeleportSequence();
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }

    private void StartTeleportSequence(){
        isTeleporting = true;
        fadeCanvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
        {
            TeleportPlayer();
            fadeCanvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
            {
                isTeleporting = false;
            });
        });
    }

    private void TeleportPlayer()
    {
        FindObjectOfType<SoundManager>().Play("Vent");
        Debug.Log($"VentInteract: Teleporting player to {teleportTarget.position}");
        player.position = teleportTarget.position;     
    }
}
