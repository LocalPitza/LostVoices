using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    [SerializeField] private PlayerInventory inventory;
    public List<GameObject> turnOn = new List<GameObject>();
    [SerializeField ]private bool turnedOn = false;

    public GameObject detector;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    private void Update() {

        if(inventory.HasItem("MotionSensor"))
        {
            detector.SetActive(true);
            if(!turnedOn)
            {
                TurnOffObjects();
            }
            
        }

    }

    void TurnOffObjects()
    {
        turnedOn = true;
        foreach (var gameObject in turnOn)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
