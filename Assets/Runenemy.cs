using System.Collections;
using Unity.Collections;
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
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;
    private bool isAware = false;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool die = false;
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

    public void ResetEnemy()
    {
        boxCollider.enabled = true;
        circleCollider.enabled = true;
        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.linearVelocity = Vector2.zero;  // Dừng mọi di chuyển
        rb.angularVelocity = 0f;      // Dừng mọi xoay
        rb.simulated = true;          // Đảm bảo Rigidbody hoạt động lại
        rb.gravityScale = 1f;         // Đặt lại trọng lực nếu bị thay đổi

        isAware = false;
        isMoving = false;
        isJumping = false;
        die = false;

        anim.Play("Idle"); // Reset animation về trạng thái ban đầu
        gameObject.SetActive(true); // Đảm bảo enemy được kích hoạt lại
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
        // Nếu chạm vào Water → Nhảy lên rồi chết
        if (rb.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            anim.Play("Die");
            die = true;
            rb.linearVelocity = Vector2.zero; // Dừng di chuyển
            boxCollider.enabled = false;
            circleCollider.enabled = false;

            Debug.Log("Enemy Died in Water...");
            return;
        }

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

    

    public void Destroyit()
    {
        gameObject.SetActive(false);
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
