using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Runenemy : MonoBehaviour
{
    public SeeAwake SeeAwake;
    public Transform player;
    public float moveSpeed = 3f;
    public float jumpForce = 6f;
    public float lookDuration = 3f;
    public LayerMask groundLayer; // Xác định đâu là mặt đất

    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;
    
    

    private bool isMoving = false;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool shouldStopAfterBox = false; // Biến này giúp Enemy tiếp tục chạy đến khi va vào Box
    private Vector2 moveDirection;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);

        if (isMoving && !isJumping)
        {
            rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        }

        if (isGrounded)
        {
            isJumping = false;
        }

        // Nếu Enemy đã va vào Box và đã kết thúc nhảy → Chuyển về Idle
        if (!isJumping && shouldStopAfterBox)
        {
            anim.Play("Idle");
            shouldStopAfterBox = false; // Reset cờ
            Debug.Log("Enemy transitions to Idle");
        }
    }

    public void ResetEnemy()
    {
        boxCollider.enabled = true;
        circleCollider.enabled = true;
        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = true;
        rb.gravityScale = 1f;

        isMoving = false;
        isJumping = false;
        shouldStopAfterBox = false;

        anim.Play("Idle");
        gameObject.SetActive(true);
    }

    public void OnPlayerEnterAware()
    {
        if (SeeAwake.awareCount > 0)
        {
            StartCoroutine(WaitForAwakeAnimation());
            Debug.Log("Player in Range, Enemy Preparing...");
        }
    }

    private IEnumerator WaitForAwakeAnimation()
    {
        yield return new WaitForSeconds(lookDuration);

        if (SeeAwake.awareCount > 0)
        {
            StartMovingTowardsPlayer();
        }
    }

    void StartMovingTowardsPlayer()
    {
        isMoving = true;
        moveDirection = (player.position.x > transform.position.x) ? Vector2.right : Vector2.left;
        transform.localScale = new Vector3(moveDirection.x, 1, 1); // Lật hướng Enemy
        anim.Play("Run");
        Debug.Log("Enemy Running...");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            anim.Play("Die");
            rb.linearVelocity = Vector2.zero;
            boxCollider.enabled = false;
            circleCollider.enabled = false;
            Debug.Log("Enemy Died in Water...");
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            rb.linearVelocity = new Vector2(-moveDirection.x * 2f, jumpForce);
            isMoving = false;
            anim.Play("Look");
            shouldStopAfterBox = true; // Đánh dấu cần dừng hẳn sau khi va vào Box

            Debug.Log("Enemy Jumping...");
        }
    }

    public void Destroyit()
    {
        gameObject.SetActive(false);
    }

    public void OnPlayerExitAware()
    {
        // Nếu Player rời vùng Aware nhưng Enemy đang chạy → Tiếp tục chạy đến khi va vào Box
        if (isMoving && anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            Debug.Log("Player left, but Enemy keeps running until it hits a Box...");
        }
        else
        {
            StopMoving();
        }
    }

    void StopMoving()
    {
        isMoving = false;
        rb.linearVelocity = Vector2.zero;
        anim.Play("Idle");
        Debug.Log("Enemy Stopped Moving...");
    }
}