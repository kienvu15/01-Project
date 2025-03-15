using System.Collections;
using UnityEngine;

public class Runenemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float jumpForce = 6f;
    public float lookDuration = 3f;
    public LayerMask groundLayer; // Xác định đâu là mặt đất

    private Rigidbody2D rb;
    private Animator anim;
    private bool isAware = false;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool isGrounded = false;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Kiểm tra Enemy có đang chạm đất không
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);

        // Nếu đang di chuyển, cập nhật linearVelocity
        if (isMoving && !isJumping)
        {
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        }
        if (isGrounded)
        {
            isJumping = false;
        }

    }

    public void OnPlayerEnterAware()
    {
        if (!isAware)
        {
            isAware = true;
            StartCoroutine(WaitForAwakeAnimation());
            Debug.Log("Player in Range");
        }
    }

    private IEnumerator WaitForAwakeAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        if (isAware) // Nếu Player vẫn ở trong Aware
        {
            StartMovingTowardsPlayer();
        }
    }

    void StartMovingTowardsPlayer()
    {
        isMoving = true;
        // Xác định hướng di chuyển
        moveDirection = (player.position.x > transform.position.x) ? Vector2.right : Vector2.left;
        transform.localScale = new Vector3(moveDirection.x, 1, 1); // Lật hướng Enemy
        anim.Play("Run");
        Debug.Log("Enemy Running...");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu Enemy đang chạy và chạm vào collider KHÔNG PHẢI Ground thì nhảy lên
        if (collision.gameObject.CompareTag("Box"))
        {

            rb.linearVelocity = new Vector2(-moveDirection.x * 2f, jumpForce);
            isMoving = false;

            anim.Play("Look");
            Debug.Log("Enemy Jumping...");
            StartCoroutine(WaitForLookAnimation());
        }
    }

    private IEnumerator WaitForLookAnimation()
    {
        Debug.Log("Onlock");
        yield return new WaitForSeconds(lookDuration);

        if (isAware)
        {
            isJumping = false;
            StartMovingTowardsPlayer();
            isMoving = true;// Nếu Player vẫn ở trong Aware, tiếp tục di chuyển
        }
        else
        {
            anim.Play("Idle"); // Nếu Player đã rời đi, mới chuyển về Idle
        }
    }



    public void OnPlayerExitAware()
    {
        isAware = false; // Đánh dấu rằng Player đã rời đi, nhưng không dừng ngay lập tức
    }
}
