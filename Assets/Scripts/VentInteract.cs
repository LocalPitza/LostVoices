using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VentInteract : Interactable
{
    public Vector3 teleportTarget;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;

    [SerializeField] private Transform player;
    [SerializeField] private bool isTeleporting = false;

    private void Start()
    {
        StartCoroutine(AssignPlayer());
    }
    private IEnumerator AssignPlayer()
    {
        while (GameObject.FindGameObjectWithTag("Player") == null)
        {
            yield return null;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
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

        FirstPersonController.instance.CanMove = false;

        fadeCanvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
        {
            TeleportPlayer();
            fadeCanvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
            {
                isTeleporting = false;
                FirstPersonController.instance.CanMove = true;
                Debug.Log("VentInteract: Teleport sequence completed.");
            });
        });
    }

    private void TeleportPlayer()
    {
        if (player == null)
        {
            Debug.LogError("VentInteract: Player reference is missing!");
            return;
        }

        FindObjectOfType<SoundManager>().Play("Vent");
        Debug.Log($"VentInteract: Teleporting player to {teleportTarget}");

        player.DOMove(teleportTarget, 0.2f, true)
            .OnComplete(() => Debug.Log("VentInteract: Player teleportation completed."))
            .OnKill(() =>      
            {
            //Debug.LogWarning("VentInteract: Player teleportation interrupted.");
            player.position = teleportTarget;
            });
    }
}
