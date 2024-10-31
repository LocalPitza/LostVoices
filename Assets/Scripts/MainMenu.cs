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
    public float fadeDuration = 1.5f;

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

        yield return new WaitForSeconds(fadeDuration);

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
