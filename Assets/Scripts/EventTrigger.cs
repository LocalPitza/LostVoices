using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public List<GameObject> _triggers = new List<GameObject>();
    public List<GameObject> _turnOff = new List<GameObject>();
    [SerializeField]private bool triggerActive = false;
    private void OnTriggerEnter(Collider other) {
        if(!triggerActive)
        {
            ActivateEvent();
        }
    }
    public void ActivateEvent()
    {
        triggerActive = true;
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
}
