using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioTriggerFade : MonoBehaviour
{
    public AudioSource audioSource;
    public float maxAudio;
    public float minAudio;
    public float fadeDuration = 1.5f;
    public bool fadeOutOnce = false;

    [SerializeField] private bool hasFadedOut = false;
    private Coroutine currentFadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeOutOnce && hasFadedOut) return;

            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);

            currentFadeCoroutine = StartCoroutine(FadeAudio(minAudio)); 
            hasFadedOut = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && (!fadeOutOnce || hasFadedOut))
        {
            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);

            currentFadeCoroutine = StartCoroutine(FadeAudio(maxAudio));
        }
    }

    private IEnumerator FadeAudio(float targetVolume)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / fadeDuration);
            yield return null;
        }

        // Ensure final volume matches targetVolume
        audioSource.volume = targetVolume;
    }
}
