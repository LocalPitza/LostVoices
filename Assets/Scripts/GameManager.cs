using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    
    private bool isPaused = false;
    public GameObject[] pauseMenuObjects;
    public AudioSource _audio;
    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1500, 500);
    }

    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        foreach (var gameObject in pauseMenuObjects)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
        Time.timeScale = 1f; // Resume the game time
        FirstPersonController.instance.CanMove = true; // Allow player movement
        FirstPersonController.instance.canMouseLook = true;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        _audio.UnPause();
        
    }

    private void Pause()
    {
        foreach (var gameObject in pauseMenuObjects)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
        }
        Time.timeScale = 0f; // Freeze the game time
        FirstPersonController.instance.CanMove = false; // Disable player movement
        FirstPersonController.instance.canMouseLook = false;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
        _audio.Pause();
        
    }
    
}
