using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject pauseMenu;
    public CanvasGroup canvasFade;
    public CanvasGroup secondCanvasFade;
    public AudioSource backgroundMusic1;
    public AudioSource backgroundMusic2;
    public float fadeDuration = 1.5f;
    public float waitTimeAfterFade = 1.0f;

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

    public void OpenSoundSettings()
    {
        Debug.Log("Opening Sound Settings");
    }

    public void OpenGraphicsSettings()
    {
        Debug.Log("Opening Graphics Settings");
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

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
}
