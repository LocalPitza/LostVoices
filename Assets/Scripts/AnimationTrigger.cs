using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField]private Animator animator;

    private void OnTriggerEnter(Collider other)
    {

        {
            if (animator != null)
            {
                animator.SetBool("triggerMove", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (animator != null)
        {
            animator.SetBool("triggerMove", false);
        }
    }
}
