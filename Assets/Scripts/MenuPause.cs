using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject pauseMenu;

    public Button returnButton;
    public Button soundButton;
    public Button graphicsButton;
    public Button mainMenuButton;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnReturnButtonClicked()
    {
        FindObjectOfType<GameManager>().Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnReturnButtonClicked();
        }
    }

    private void OpenSoundSettings()
    {
        Debug.Log("Opening Sound Settings");
    }

    private void OpenGraphicsSettings()
    {
        Debug.Log("Opening Graphics Settings");
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu");
    }
}
