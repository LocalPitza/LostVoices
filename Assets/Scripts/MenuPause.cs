using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    private GameManager gameManager;
    public CanvasGroup canvasFade;
    public CanvasGroup secondCanvasFade;
    public AudioSource backgroundMusic1;
    public AudioSource backgroundMusic2;
    public float fadeDuration = 1.5f;
    public float waitTimeAfterFade = 1.0f;
    public GameObject settingsPanel;
    bool settingPanelOpened;
    public GameObject audioPanel;
    public GameObject videoPanel;
    public GameObject controlPanel;
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
        if (Input.GetKeyDown(KeyCode.Escape) && !settingPanelOpened)
        {
            OnReturnButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(!settingPanelOpened)
            {
                settingsPanel.SetActive(false);
            }
            else
            {
                audioPanel.SetActive(false);
                videoPanel.SetActive(false);
                controlPanel.SetActive(false);
                settingPanelOpened = false;
            }
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void LoadSave()
    {
        Debug.Log("Loading Last Save");
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Are you sure?");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void OpenAudio()
    {
        videoPanel.SetActive(false);
        controlPanel.SetActive(false);
        audioPanel.SetActive(true);
        settingPanelOpened = true;
    }

    public void OpenVideo()
    {
        audioPanel.SetActive(false);
        controlPanel.SetActive(false);
        videoPanel.SetActive(true);
        settingPanelOpened = true;
    }

    public void OpenControls()
    {
        audioPanel.SetActive(false);
        videoPanel.SetActive(false);
        controlPanel.SetActive(true);
        settingPanelOpened = true;
    }

    /*
    private IEnumerator FadeAndSwitchScene()
    {

        secondCanvasFade.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        canvasFade.DOFade(1, fadeDuration).SetEase(Ease.InOutQuad);

        backgroundMusic1.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        backgroundMusic2.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(fadeDuration);

        yield return new WaitForSeconds(waitTimeAfterFade);

        SceneManager.LoadScene("MainMenu");
    }
    */
}
