using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTrigger : MonoBehaviour
{
    public Rigidbody targetRigidbody; // The Rigidbody to activate
    public float activeDuration = 2f;  // Duration to keep the Rigidbody active
    public AudioSource audioSource;

    private void Start()
    {
        // Ensure the Rigidbody is initially disabled (kinematic)
        if (targetRigidbody != null)
        {
            targetRigidbody.isKinematic = true; // Set to kinematic to disable physics
        }
        else
        {
            Debug.LogWarning("Target Rigidbody is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetRigidbody != null)
        {
            // Activate the Rigidbody
            targetRigidbody.isKinematic = false; // Ensure it is not kinematic
            targetRigidbody.WakeUp(); // Wake up the Rigidbody if it was asleep
            audioSource.Play();
            
            // Start the coroutine to disable it after a delay
            StartCoroutine(DisableRigidbodyAfterDelay(activeDuration));
        }
    }

    private IEnumerator DisableRigidbodyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Disable the Rigidbody
        targetRigidbody.isKinematic = true; // Set it back to kinematic
    }
}
