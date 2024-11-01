using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LockerScript : Interactable
{
    [SerializeField] private Transform hidingPoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform player;
    [SerializeField] private float lockerFadeDuration = 0.8f;
    [SerializeField] private float holdTimeToExit = 2.0f;

    private Collider playerCollider;
    private bool hiding;
    private float holdTimer;

    private void Start()
    {
        StartCoroutine(AssignPlayerAndCollider());
    }
    private void Update() 
    {
        if (hiding)
        {
            UIInteract.Instance.ShowText("Hold H to leave");

            if (Input.GetKey(KeyCode.H))
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= holdTimeToExit)
                {
                    FadeManager.Instance.SetFadeDuration(lockerFadeDuration);
                    StartCoroutine(ExitLocker());
                    holdTimer = 0f; // Reset the hold timer
                }
            }
            else if (Input.GetKeyUp(KeyCode.H))
            {
                holdTimer = 0f; // Reset hold timer if the key is released
            }
        }
    }

    private IEnumerator AssignPlayerAndCollider()
    {
        while (GameObject.FindGameObjectWithTag("Player") == null)
        {
            yield return null;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCollider = player.GetComponent<Collider>();
    }

    public override void OnFocus()
    {
        if(!hiding)
        {
            UIInteract.Instance.ShowText("Hide");
        }
        else
        {
            UIInteract.Instance.ShowText("Hold H to leave");
        }
        
    }

    public override void OnInteract()
    {
        FadeManager.Instance.SetFadeDuration(lockerFadeDuration);
        StartCoroutine(FadeAndHidePlayer());
    }

    private IEnumerator FadeAndHidePlayer()
    {
        FadeManager.Instance.FadeIn();

        yield return new WaitForSeconds(lockerFadeDuration);

        hiding = true;
        playerCollider.enabled = false;
        FindObjectOfType<SoundManager>().Play("LockerEnter");
        player.position = hidingPoint.transform.position;
        FirstPersonController.instance.CanMove = false;

        yield return new WaitForSeconds(lockerFadeDuration);
        FadeManager.Instance.FadeOut();
    }

    private IEnumerator ExitLocker()
    {
        FadeManager.Instance.FadeIn();
        yield return new WaitForSeconds(lockerFadeDuration);

        hiding = false;
        FindObjectOfType<SoundManager>().Play("LockerExit");

        playerCollider.enabled = false;
        player.DOMove(exitPoint.position, 0.5f)
            .OnComplete(() => 
            {
                playerCollider.enabled = true;
                FirstPersonController.instance.CanMove = true;
                FadeManager.Instance.FadeOut();
                UIInteract.Instance.HideText();
            });

        yield return new WaitForSeconds(0.5f);
    }

    public override void OnLoseFocus()
    {
        UIInteract.Instance.HideText();
    }
}
