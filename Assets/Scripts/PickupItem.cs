using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    [SerializeField] private string itemID; // Unique ID for the item
    [SerializeField] private string interactText = "Pick up"; // Text displayed when focusing on the item

    public override void OnFocus()
    {
        // Show the interact text on the UI
        UIInteract.Instance.ShowText(interactText);
    }

    public override void OnInteract()
    {
        // Find the PlayerInventory component on the player
        PlayerInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            FindObjectOfType<SoundManager>().Play("pickup");
            inventory.AddItem(itemID); // Add item to player's inventory
            Debug.Log($"Picked up item: {itemID}");
            
            // Optionally, destroy the item GameObject after pickup
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("PlayerInventory not found on the Player GameObject.");
        }
    }

    public override void OnLoseFocus()
    {
        // Hide the interact text on the UI
        UIInteract.Instance.HideText();
    }
}
