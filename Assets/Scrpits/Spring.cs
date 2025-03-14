using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float force = 15f; // Lực nảy có thể tùy chỉnh
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("spring", true);

            // Lấy Rigidbody2D của Player
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vận tốc trước khi nhảy
                rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("spring", false);
        }
    }
}
