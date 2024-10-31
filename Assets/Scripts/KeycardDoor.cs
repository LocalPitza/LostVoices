using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class KeycardDoor : Interactable
{
    public GameObject Door;
    public GameObject keycardIndicator; // Direct reference to the indicator GameObject to change material
    public Material accessibleMaterial; // Material when keycard is available
    public Material lockedMaterial; // Material when locked
    public string requiredKeycardID = "Keycard"; // ID for the keycard
    public bool isOpen;
    public bool useOnce;
    public bool moveSnap;
    public float moveSpeed;
    public AudioSource _audio;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    [SerializeField] private bool hasBeenUsed = false;
    private bool hasKeycard = false;
    private string _text = "Interact"; // Interaction text

    private Renderer keycardIndicatorRenderer;

    private void Start()
    {
        // Get the Renderer of the assigned indicator GameObject
        if (keycardIndicator != null)
        {
            keycardIndicatorRenderer = keycardIndicator.GetComponent<Renderer>();
            if (keycardIndicatorRenderer != null && lockedMaterial != null)
            {
                keycardIndicatorRenderer.material = lockedMaterial; // Set initial locked material
            }
        }
    }

    public override void OnFocus()
    {
        UpdateInteractText();
        UIInteract.Instance.ShowText(_text);
    }

    public override void OnInteract()
    {
        if (useOnce && hasBeenUsed)
            return;

        // Check for keycard in the player's inventory
        playerInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<playerInventory>();
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
            // Update the material on the assigned indicator GameObject if player has keycard
            if (keycardIndicatorRenderer != null && accessibleMaterial != null)
            {
                keycardIndicatorRenderer.material = accessibleMaterial;
            }

            // Toggle door state
            _audio.Play();
            Door.transform.DOMove(isOpen ? start : end, moveSpeed, moveSnap);
            isOpen = !isOpen;

            // Mark as used if useOnce is true
            if (useOnce) hasBeenUsed = true;
        }
    }
}
