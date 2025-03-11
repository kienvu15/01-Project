using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioClip jumpSound;
    private AudioSource audioSource;

    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private float gravity;

    

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

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravity = rb.gravityScale;
        audioSource = GetComponent<AudioSource>();
    }



    public float glideGravityScale = 0.5f; // Trọng lực khi lượn
    public float fallGravityScale = 2f;

    // Cập nhật trong `Update()`
    void Update()
    {
        Grounded();
        


        
            Move();

        Glide();
        Jump();
        
        CheckVerticalState();
        UpdateJumpVariales();
    }

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float airMoveSpeed = 6f; // Tốc độ di chuyển khi ở trên không
    private float moveX;
    private bool isFacingRight = true;

    public void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        // Nếu đang ở trên không, dùng airMoveSpeed
        float currentSpeed = isGround ? moveSpeed : airMoveSpeed;

        Vector2 movement = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;

        anim.SetBool("isRunning", moveX != 0);

        if ((moveX > 0 && !isFacingRight) || (moveX < 0 && isFacingRight))
        {
            Flip();
        }
    }


    void Glide()
    {
        if (jumping)
        {
            if (Input.GetKey(KeyCode.Space)) // Giữ Space để lượn
            {
                rb.gravityScale = glideGravityScale;
                anim.SetBool("Dive", true); // Bật animation Dive
            }
            else // Thả Space thì rơi nhanh hơn
            {
                rb.gravityScale = fallGravityScale;
                anim.SetBool("Dive", false); // Tắt animation Dive
            }
        }
        if (!isGround && rb.linearVelocity.y < 0) // Chỉ kích hoạt khi đang rơi xuống sau khi nhảy
        {
            if (Input.GetKey(KeyCode.Space)) // Giữ Space để lượn
            {
                rb.gravityScale = glideGravityScale;
                anim.SetBool("Dive", true); // Bật animation Dive
            }
            else // Thả Space thì rơi nhanh hơn
            {
                rb.gravityScale = fallGravityScale;
                anim.SetBool("Dive", false); // Tắt animation Dive
            }
        }
        else // Khi chạm đất hoặc đang nhảy lên thì giữ trọng lực mặc định
        {
            rb.gravityScale = gravity;
            anim.SetBool("Dive", false); // Đảm bảo tắt Dive khi không lượn
        }
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
                anim.SetBool("Dive", false);
            }
            else if (linearVelocityY > 0.1f && isDoublejump)
            {
                anim.SetTrigger("Jump2");
                anim.SetBool("jump2", true);
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", false);
                anim.SetBool("Dive", false);
                anim.SetBool("isRunning", false);
            }
            else if (linearVelocityY < -0.1f)
            {
                if (Input.GetKey(KeyCode.Space)) // Nếu đang lượn thì không bật Fall
                {
                    anim.SetBool("Dive", true);
                    anim.SetBool("Fall", false);
                }
                else
                {
                    anim.SetBool("Dive", false);
                    anim.SetBool("Fall", true);
                }
            }
        }
        else
        {
            anim.SetBool("jump2", false);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            anim.SetBool("Dive", false); // Khi chạm đất, tắt Dive
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