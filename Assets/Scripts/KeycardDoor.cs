using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class KeycardDoor : Interactable
{
    public GameObject Door;
    public GameObject keycardIndicator;             // Indicator for the keycard requirement
    public Material accessibleMaterial;             // Material for when the door is accessible
    public Material lockedMaterial;                 // Material for when the door is locked
    public ParticleSystem[] doorParticles;          // Particle systems for opening effects
    public string requiredKeycardID = "Keycard";    // ID for the keycard
    public bool isOpen = false;                     // Track door open state
    public bool useOnce = false;                    // Whether the door can be used only once
    public bool moveSnap = false;                   // Snap door movement
    public float moveSpeed = 1.0f;                  // Door movement speed
    public float fadeOutDuration = 2.0f;            // Duration for particle fade-out
    public AudioSource _audio;                      // Audio for door interaction
    [SerializeField] private Vector3 start;         // Starting position of the door
    [SerializeField] private Vector3 end;           // Ending position of the door
    [SerializeField] private bool hasBeenUsed = false;

    private bool hasKeycard = false;
    private string _text = "Interact";              // Interaction prompt text
    private Renderer keycardIndicatorRenderer;      // Renderer for keycard indicator

    private void Start()
    {
        // Initialize the keycard indicator's material
        if (keycardIndicator != null)
        {
            keycardIndicatorRenderer = keycardIndicator.GetComponent<Renderer>();
            if (keycardIndicatorRenderer != null && lockedMaterial != null)
            {
                keycardIndicatorRenderer.material = lockedMaterial;
            }
        }

        // Ensure all particle systems are stopped and disabled at the start
        foreach (var particle in doorParticles)
        {
            particle.Stop();
            particle.Clear();
            particle.gameObject.SetActive(false);
        }
    }

    public override void OnFocus()
    {
        UpdateInteractText();
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnInteract()
    {
        if (useOnce && hasBeenUsed) return;

        // Check for keycard in the player's inventory
        PlayerInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        if (inventory != null && inventory.HasItem(requiredKeycardID))
        {
            hasKeycard = true;
            UpdateDoorStatus();
        }
        else
        {
            Debug.Log("Keycard required to open this door.");
        }
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }

    private void UpdateInteractText()
    {
        _text = isOpen ? "Interact" : (hasKeycard ? "Interact" : "Requires Keycard");
    }

    private void UpdateDoorStatus()
    {
        if (hasKeycard)
        {
            // Update the keycard indicator material if the player has the keycard
            if (keycardIndicatorRenderer != null && accessibleMaterial != null)
            {
                keycardIndicatorRenderer.material = accessibleMaterial;
            }

            // Play audio and open or close the door
            _audio.Play();
            Door.transform.DOMove(isOpen ? start : end, moveSpeed, moveSnap);
            isOpen = !isOpen;

            // Trigger particle effects if door is opened
            if (isOpen)
            {
                foreach (var particle in doorParticles)
                {
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }
            }
            else
            {
                // Gradually fade out particle effects if door is closed
                foreach (var particle in doorParticles)
                {
                    StartCoroutine(FadeOutAndDisableParticle(particle));
                }
            }

            // Mark door as used if only usable once
            if (useOnce) hasBeenUsed = true;
        }
    }

    private IEnumerator FadeOutAndDisableParticle(ParticleSystem particle)
    {
        // Retrieve the emission module and its initial rate
        var emission = particle.emission;
        float initialRate = emission.rateOverTime.constant;
        
        // Gradually reduce the emission rate over the fade-out duration
        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            float rate = Mathf.Lerp(initialRate, 0, t / fadeOutDuration);
            emission.rateOverTime = rate;
            yield return null;
        }

        // Ensure particle system is stopped and then disable it
        emission.rateOverTime = 0;
        particle.Stop();
        particle.gameObject.SetActive(false);
    }
}
