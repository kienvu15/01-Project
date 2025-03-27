using UnityEngine;

public class Aware : MonoBehaviour
{
    public Animator targetAnimator;
    public string animationName = "Shake";

    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (targetAnimator != null)
            {
                targetAnimator.Play(animationName);
            }
        }
    }

    // Reset lại trạng thái ban đầu của Aware
    public void ResetAware()
    {
        if (targetAnimator != null)
        {
            targetAnimator.Play("Idle", 0, 0f); // Reset về animation "Idle"
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            Invoke(nameof(ReenableCollider), 0.5f); // Đợi 0.5s rồi bật lại
        }
    }

    private void ReenableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

}
