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
            Debug.LogError("Set");
        }
    }

    void OnDestroy()
    {
        if (animator != null)
        {
            animator.SetBool("Entry", false);
            Debug.LogError("Destroyed");
        }
    }
}
