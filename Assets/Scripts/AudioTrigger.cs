using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public string audioName;
    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<SoundManager>().Play(audioName);
        }
        
    }
}
