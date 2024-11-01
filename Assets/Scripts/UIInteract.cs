using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
public class UIInteract : MonoBehaviour
{
    public static UIInteract Instance;
    public TextMeshProUGUI interactText;
    public float fadeSpeed;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Update()
    {
        canvasGroup.DOFade(0, fadeSpeed);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        canvasGroup = interactText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = interactText.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
    }

    public void ShowText(string text)
    {
        interactText.text = text;
        canvasGroup.DOFade(1, fadeSpeed);
    }

    public void HideText()
    {
        canvasGroup.DOFade(0, fadeSpeed);
    }
}
