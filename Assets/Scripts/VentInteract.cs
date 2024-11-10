using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VentInteract : Interactable
{
    public Vector3 teleportTarget;
    public float fadeDuration = 0.5f;

    [SerializeField] private Transform player;
    private bool isTeleporting = false;

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

    private void StartTeleportSequence()
    {
        isTeleporting = true;

        FirstPersonController.instance.CanMove = false;
        FadeManager.Instance.SetFadeDuration(fadeDuration);

        FadeManager.Instance.FadeIn();
        FadeManager.Instance.StartCoroutine(TeleportAfterFade());
    }

    private IEnumerator TeleportAfterFade()
    {
        yield return new WaitForSeconds(fadeDuration);

        TeleportPlayer();

        yield return new WaitForSeconds(fadeDuration);
        
        FadeManager.Instance.FadeOut();
        FirstPersonController.instance.CanMove = true;
        isTeleporting = false;
    }

    private void TeleportPlayer()
    {
        if (player == null)
        {
            Debug.LogError("VentInteract: Player reference is missing!");
            return;
        }

        FindObjectOfType<SoundManager>().Play("Vent");
        player.position = teleportTarget;
        Debug.Log("VentInteract: Player teleported.");
    }
}
