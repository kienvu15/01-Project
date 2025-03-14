using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator animator;
    Collider2D checkpointCollider;
    Rigidbody2D rb;
    private bool isActivated = false;
    public GameObject DustBlast;
    public FlashEffect flashEffect;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        checkpointCollider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && collision.CompareTag("Player"))
        {
            Instantiate(DustBlast, rb.transform.position, Quaternion.Euler(0, 0, 90));
            StartCoroutine(flashEffect.StartFlash());
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.respawnPoint.position = transform.position; // Cập nhật vị trí respawn
            }

            animator.Play("checkpoint0");
            isActivated = true;
        }
    }

    public void StayOnAnimation()
    {
        animator.Play("checkpoint");
        checkpointCollider.enabled = false;
    }
}
