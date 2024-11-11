using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    [SerializeField] private string itemID;
    [SerializeField] private string interactText = "Press F to Examine";
    public bool isMandatoryPickup = false;

    public string ItemID => itemID;

    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public override void OnFocus()
    {
        UIInteract.Instance.ShowText(interactText);
    }

    public override void OnInteract()
    {
        //its on the firstpersoncontroller script
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }
}
