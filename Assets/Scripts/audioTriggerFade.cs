using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTriggerFade : MonoBehaviour
{
    public AudioSource audioSource;          // Reference to the audio source to fade
    public float fadeDuration = 1.5f;        // Duration for fading in and out
    public bool fadeOutOnce = false;         // If true, only fades out once on the first trigger

    private bool hasFadedOut = false;        // Tracks if the audio has already faded out
    private Coroutine currentFadeCoroutine;  // Keeps track of the current coroutine

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the audio should fade out only once and has already faded out
            if (fadeOutOnce && hasFadedOut) return;

            // Start fading out audio, stop any ongoing fade-in coroutine
            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);

            currentFadeCoroutine = StartCoroutine(FadeAudio(0f));  // Fade out
            hasFadedOut = true;  // Mark that the audio has faded out once
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && (!fadeOutOnce || !hasFadedOut))
        {
            // Start fading in audio, stop any ongoing fade-out coroutine
            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);

            currentFadeCoroutine = StartCoroutine(FadeAudio(1f));  // Fade in
        }
    }

    private IEnumerator FadeAudio(float targetVolume)
    {
        float startVolume = audioSource.volume;  // Record the current volume

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            // Smoothly interpolate volume from startVolume to targetVolume over fadeDuration
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / fadeDuration);
            yield return null;
        }

        // Ensure final volume matches targetVolume
        audioSource.volume = targetVolume;
    }
}
