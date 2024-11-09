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
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(90000, 500);
    }

    void Update()
    {
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
        Time.timeScale = 1f;
        FirstPersonController.instance.CanMove = true;
        FirstPersonController.instance.canMouseLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        Time.timeScale = 0f;
        FirstPersonController.instance.CanMove = false;
        FirstPersonController.instance.canMouseLook = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _audio.Pause();
        
    }
    
}
