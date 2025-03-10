using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AudioClip jumpSound;
    private AudioSource audioSource;

    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private float gravity;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;
    private float moveX;
    private bool isFacingRight = true;

    [Header("Jump")]
    private bool jumping = false;
    public float jumpForce = 13.5f;
    private int jumpBufferCounter = 0;
    public int jumpBufferFrames;
    private float coyoteTimeCount = 0;
    public float coyoteTime;
    private float airjumpCount = 0;
    public float maxAirJump;
    private bool isGround;

    [Header("WallJump")]
    private bool isWallSliding;
    [SerializeField] public float wallSlidingSpeed = 1.5f; // Giảm tốc độ rơi khi trượt
    [SerializeField] private LayerMask WallLayer;
    [SerializeField] private Transform wallCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravity = rb.gravityScale;
        audioSource = GetComponent<AudioSource>();
    }

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForce = 13.5f;
    [SerializeField] private float wallJumpPushForce = 8f;
    [SerializeField] private float wallJumpTime = 0.2f; // Thời gian vô hiệu hóa điều khiển sau Wall Jump
    private bool isWallJumping = false;
    private float wallJumpTimer = 0f;

    

    // Cập nhật trong `Update()`
    void Update()
    {
        Grounded();
        IsWalled();

        
        if (!isWallJumping) // Không di chuyển nếu đang Wall Jump
        {
            Move();
        }

        Jump();
        WallSlide();
        WallJump();
        CheckVerticalState();
        UpdateJumpVariales();
    }

    
    public void WallJump()
    {
        if (isWallSliding && Input.GetKeyDown(KeyCode.Space))
        {
            isWallJumping = true;
            wallJumpTimer = wallJumpTime;

            // Xác định hướng bật nhảy (ngược hướng facing)
            float jumpDirection = isFacingRight ? -1f : 1f;

            rb.linearVelocity = new Vector2(jumpDirection * wallJumpPushForce, jumpForce);

            // Reset số lần nhảy trên không (cho phép Double Jump sau Wall Jump)
            airjumpCount = 0;

            // Giảm gravityScale tạm thời để rơi chậm hơn sau Wall Jump
            rb.gravityScale = gravity * 0.3f;
            Invoke(nameof(ResetGravity), 1f); // Đưa gravityScale về bình thường sau 1 giây

            // Lật hướng nhân vật nếu cần
            if ((jumpDirection < 0 && isFacingRight) || (jumpDirection > 0 && !isFacingRight))
            {
                Flip();
            }
        }

        // Giữ trạng thái Wall Jump trong một khoảng thời gian ngắn trước khi cho phép điều khiển lại
        if (isWallJumping)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                isWallJumping = false;
            }
        }
    }

    // Hàm reset gravity về mặc định
    private void ResetGravity()
    {
        rb.gravityScale = gravity;
    }





    private bool Grounded()
    {
        bool grounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (grounded)
        {
            isGround = true;
            isDoublejump = false;
        }
        else
        {
            isGround = false;
        }

        return grounded;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.3f, WallLayer);
    }

    private void WallSlide()
    {
        bool movingAgainstWall = (isFacingRight && moveX < 0) || (!isFacingRight && moveX > 0);

        if (IsWalled() && !Grounded() && rb.linearVelocity.y < 0)
        {
            if (!movingAgainstWall)
            {
                isWallSliding = true;
                rb.gravityScale = 0.5f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlidingSpeed);
            }
            else
            {
                // Nếu di chuyển ngược lại, thoát khỏi trạng thái Wall Slide
                isWallSliding = false;
                rb.gravityScale = gravity;

                // 🌟 Cho phép Double Jump ngay khi thoát khỏi tường
                if (airjumpCount == 0)
                {
                    airjumpCount = 1; // Reset lại để người chơi có thể nhảy một lần
                }
            }
        }
        else
        {
            isWallSliding = false;
            rb.gravityScale = gravity;
        }

        anim.SetBool("isWallSliding", isWallSliding);
    }


    public void Move()
    {
        
        moveX = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;

        anim.SetBool("isRunning", moveX != 0);
        if ((moveX > 0 && !isFacingRight) || (moveX < 0 && isFacingRight))
        {
            Flip();
        }
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }



    public bool isDoublejump = false;
    public void Jump()
    {
        // Nếu người chơi nhả phím Space khi đang nhảy lên, cắt ngắn lực nhảy (jump cut)
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            jumping = false;
        }

        // Nếu chưa nhảy
        if (!jumping)
        {
            // Nhảy bình thường (Jump Buffer + Coyote Time)
            if (jumpBufferCounter > 0 && coyoteTimeCount > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumping = true;
                jumpBufferCounter = 0;
                isDoublejump = false;
                airjumpCount = 0; // Reset số lần nhảy trên không
            }
            // Xử lý Wall Jump
            else if (IsWalled() && Input.GetKeyDown(KeyCode.Space))
            {
                WallJump(); // Gọi Wall Jump
            }
            // Xử lý Double Jump
            else if (!Grounded() && airjumpCount < maxAirJump && Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumping = true;
                isDoublejump = true;
                airjumpCount++;
            }
        }
    }


    public void CheckVerticalState()
    {
        float linearVelocityY = rb.linearVelocity.y;
        if (!Grounded())
        {
            if (linearVelocityY > 0.1f && !isDoublejump)
            {
                anim.SetBool("Jump", true);
                anim.SetBool("Fall", false);
            }
            else if (linearVelocityY > 0.1f && isDoublejump)
            {
                anim.SetTrigger("Jump2");
                anim.SetBool("jump2", true);
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", false);
                anim.SetBool("isRunning", false);
            }
            else if (linearVelocityY < -0.1f)
            {
                anim.SetBool("jump2", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", true);
            }
        }
        else
        {
            anim.SetBool("jump2", false);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
        }
    }
    public void OnJumpComplete()
    {
        anim.ResetTrigger("Jump2");
        anim.SetBool("jump2", false);
    }
    public void UpdateJumpVariales()
    {
        if (Grounded())
        {
            jumping = false;
            coyoteTimeCount = coyoteTime;
            airjumpCount = 0;
        }
        else
        {
            coyoteTimeCount -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }
}
