using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFlicker : MonoBehaviour
{
    public Light lightSource;                 // Reference to the Light component
    public AudioSource flickerSoundSource;    // AudioSource for flickering sound
    public AudioSource initialSoundSource;     // AudioSource for the initial sound
    public float flickerInterval = 0.1f;      // Interval between flickers
    public int flickerCount = 10;             // Number of times to flicker
    public bool stopAfterFlicker = true;      // Whether flicker should stop after a certain count

    private bool isFlickering = false;        // Track whether the light is currently flickering

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the initial sound only once
            if (initialSoundSource != null && !isFlickering)
            {
                initialSoundSource.Play();
            }

            // Start flickering if not already in progress
            if (!isFlickering)
            {
                StartCoroutine(FlickerLight());
            }
        }
    }

    private IEnumerator FlickerLight()
    {
        isFlickering = true;

        for (int i = 0; i < flickerCount || !stopAfterFlicker; i++)
        {
            // Toggle the light's enabled state
            lightSource.enabled = !lightSource.enabled;

            // Play the flicker sound
            if (flickerSoundSource != null)
            {
                flickerSoundSource.Play();
            }

            // Wait for the specified interval before flickering again
            yield return new WaitForSeconds(flickerInterval);
        }

        // Ensure the light stays on after flickering stops
        lightSource.enabled = true;
        isFlickering = false;
    }
}
