using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NormalDoorScript : Interactable
{
    public GameObject Door;
    public bool isOpen;
    public bool openOnce;
    private bool usedOnce;
    public bool moveSnap;
    public float moveSpeed;
    [SerializeField] private string _text = "Interact";
    public AudioSource _audio;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    public ParticleSystem[] openDoorEffects;

    private void Start()
    {
        if (openDoorEffects != null)
        {
            foreach (var effect in openDoorEffects)
            {
                if (effect != null)
                {
                    effect.Stop();
                }
            }
        }
    }

    public override void OnFocus()
    {
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnInteract()
    {

        if (openOnce && usedOnce) return;

        if (!isOpen)
        {
            _audio.Play();
            Door.transform.DOMove(end, moveSpeed, moveSnap);
            isOpen = true;

            foreach (var effect in openDoorEffects)
            {
                if (effect != null)
                {
                    effect.Play();
                    StartCoroutine(StopEffectAfterDuration(effect));
                }
            }
        }
        else
        {
            _audio.Play();
            Door.transform.DOMove(start, moveSpeed, moveSnap);
            isOpen = false;

            foreach (var effect in openDoorEffects)
            {
                if (effect != null)
                {
                    effect.Play();
                    StartCoroutine(StopEffectAfterDuration(effect));
                }
            }
        }

        if (openOnce)
        {
            usedOnce = true;
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }
    private IEnumerator StopEffectAfterDuration(ParticleSystem effect)
    {
        yield return new WaitForSeconds(effect.main.duration);
        effect.Stop();
    }
}
