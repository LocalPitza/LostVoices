using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Generator : Interactable
{
    private bool activated;
    public List<Light> _lights = new List<Light>();
    public List<GameObject> _triggers = new List<GameObject>();
    public List<GameObject> _turnOff = new List<GameObject>();
    public float lightIntensity = 1.5f;
    public AudioSource generatorAudio;
    public AudioSource turnOnAudio;

    [Header("Camera Shake Settings")]
    public RectTransform shakeImage;
    public float shakeDuration = 0.5f;
    public float shakeStrength = 20f;
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;

    [Header("Door")]
    public GameObject Door;
    public bool moveSnap;
    public float moveSpeed;

    public Vector3 end;

    private void Start() 
    {

        GameObject[] foundLights = GameObject.FindGameObjectsWithTag("Lights/HUB");

        foreach (var lightObject in foundLights)
        {
            Light lightComponent = lightObject.GetComponent<Light>();
            if (lightComponent != null)
            {
                _lights.Add(lightComponent);
                lightComponent.intensity = 0;
            }
        }
        
    }

    public override void OnFocus()
    {
        if (!activated)
        {
            UIInteract.Instance.ShowText("Activate");
        }
        else
        {
            UIInteract.Instance.ShowText("Already Active");
        }
        
    }

    public override void OnInteract()
    {
        Door.transform.DOMove(end, moveSpeed, moveSnap);

        if (!activated)
        {
            ActivateGenerator();
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }

    private void ActivateGenerator()
    {
        activated = true;

        if (generatorAudio != null)
        {
            turnOnAudio.Play();
            generatorAudio.Play(); 
        }

        foreach (var light in _lights)
        {
            if (light != null)
            {
                light.intensity = lightIntensity;
            }
        }

        foreach (var gameObject in _triggers)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
        }
        foreach (var gameObject in _turnOff)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void ShakeScreen()
    {
        if (shakeImage != null)
        {
            // Create a shake effect on the UI image
            shakeImage.anchoredPosition = Vector2.zero; // Reset position
            shakeImage.DOShakeAnchorPos(
                shakeDuration,
                shakeStrength * Vector2.one,
                shakeVibrato,
                shakeRandomness
            );
        }
    }
}
