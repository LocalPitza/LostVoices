using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSFX : MonoBehaviour
{
    public AudioSource audioSource; // Reference to an AudioSource component
    public AudioClip[] soundEffects; // Array of sound effects to randomly choose from
    [Range(0f, 1f)]
    public float playChance = 0.5f; // The chance (0 to 1) that a sound will play each time
    public float minDelay = 1f; // Minimum delay between attempts to play sound
    public float maxDelay = 5f; // Maximum delay between attempts to play sound

    private void Start()
    {
        StartCoroutine(PlayRandomSound());
    }

    private IEnumerator PlayRandomSound()
    {
        while (true)
        {
            // Wait for a random time between minDelay and maxDelay
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Decide randomly whether to play a sound
            if (Random.value <= playChance)
            {
                // Choose a random sound effect from the array
                AudioClip clip = soundEffects[Random.Range(0, soundEffects.Length)];
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
