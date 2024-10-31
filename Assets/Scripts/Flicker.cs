using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{

    private bool isFlickering = false;
    [SerializeField] private float timeDelay;
    [SerializeField] private float flickerLength;
    [SerializeField] private AudioClip[] flickerSounds;
    private AudioSource audioSource;
    private void Start() {
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if(isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        PlayRandomFlickerSound();
        timeDelay = Random.Range(0.01f, flickerLength);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        PlayRandomFlickerSound();
        timeDelay = Random.Range(0.01f, flickerLength);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }

    private void PlayRandomFlickerSound()
    {
        if (flickerSounds != null && flickerSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, flickerSounds.Length);
            audioSource.PlayOneShot(flickerSounds[randomIndex]);
        }
    }

}
