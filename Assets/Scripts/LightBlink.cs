using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlink : MonoBehaviour
{
    [SerializeField] Light lightObject;
    [SerializeField] AudioSource lightSound;
    public float minTime;
    public float maxTime;
    public float timer;

    private void Start() {
        timer = Random.Range(minTime, maxTime);
    }

    private void Update() {
        LightFlicker();
    }

    void LightFlicker() {
        if (timer > 0)
            timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            lightObject.enabled = !lightObject.enabled;
            timer = Random.Range(minTime, maxTime);
            lightSound.Play();
        }
    }
}
