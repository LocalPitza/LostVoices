using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInventory : MonoBehaviour
{
private Dictionary<string, int> items = new Dictionary<string, int>();

    // Adds an item to the inventory by ID
    public void AddItem(string itemID)
    {
        if (items.ContainsKey(itemID))
        {
            items[itemID]++;
        }
        else
        {
            items[itemID] = 1;
        }
    }

    // Checks if the inventory contains at least one of the specified item
    public bool HasItem(string itemID)
    {
        return items.ContainsKey(itemID) && items[itemID] > 0;
    }

    // Removes an item from the inventory by ID
    public bool RemoveItem(string itemID)
    {
        if (HasItem(itemID))
        {
            items[itemID]--;
            if (items[itemID] <= 0)
            {
                items.Remove(itemID);
            }
            return true;
        }
        return false;
    }

    // For debugging: Displays inventory contents in the console
    public void PrintInventory()
    {
        foreach (var item in items)
        {
            Debug.Log($"Item: {item.Key}, Quantity: {item.Value}");
        }
    }
}
