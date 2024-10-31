using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blinking : MonoBehaviour
{
    public float phaseDuration = 1f;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;

        StartPhasing();
    }

    private void StartPhasing()
    {
        canvasGroup.DOFade(1f, phaseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
