using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup canvasFade;
    public CanvasGroup secondCanvasFade;
    public AudioSource backgroundMusic1;
    public AudioSource backgroundMusic2;
    public float fadeDuration = 1.5f;
    public float waitTimeAfterFade = 1.0f;

    private void Start()
    {
        canvasFade.alpha = 0;
        secondCanvasFade.alpha = 1;
    }

    public void startGame()
    {
        StartCoroutine(FadeAndSwitchScene());
    }

    private IEnumerator FadeAndSwitchScene()
    {

        secondCanvasFade.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        canvasFade.DOFade(1, fadeDuration).SetEase(Ease.InOutQuad);

        backgroundMusic1.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        backgroundMusic2.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(fadeDuration);

        yield return new WaitForSeconds(waitTimeAfterFade);

        SceneManager.LoadScene("MainGame");
    }

    public void openSettings()
    {
        Debug.Log("Opened Settings");
    }

    public void exitGame()
    {
        Debug.Log("Terminated Game");
    }
}
