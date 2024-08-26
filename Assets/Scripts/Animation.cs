using UnityEngine;

public class PlatformAnimator : MonoBehaviour
{
    public Animator animator;  

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Entry", true);
        }
    }

    void OnDestroy()
    {
        if (animator != null)
        {
            animator.SetBool("Entry", false);
        }
    }
}
