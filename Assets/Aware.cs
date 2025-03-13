using UnityEngine;

public class Aware : MonoBehaviour
{
    public Animator targetAnimator;
    public string animationName = "Shake"; // Tên animation trong Animator

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (targetAnimator != null)
            {
                targetAnimator.Play(animationName); // Phát animation trực tiếp
            }
        }
    }
}
