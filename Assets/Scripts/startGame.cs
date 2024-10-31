using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class startGame : MonoBehaviour
{
    public CanvasGroup canvasFade;

    public float fadeDuration = 1.5f;

    void Start()
    {
        canvasFade.alpha = 1;
        StartCoroutine(fadeStart());
    }
    private IEnumerator fadeStart()
    {
        canvasFade.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(fadeDuration);

    }
}
