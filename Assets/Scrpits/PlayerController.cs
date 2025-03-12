using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioClip jumpSound;
    private AudioSource audioSource;
    private PlayerController playerController;
    public FlashEffect flashEffect;

    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private float gravity;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMoveSpeed = 5f;
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
    public float glideGravityScale = 0.5f;
    public float fallGravityScale = 2f;
    [Space(5)]

    [Header("Object")]
    public Transform PortalFinish;
    public ParticleSystem dust;
    public Transform Foot;
    public GameObject DustBlast;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravity = rb.gravityScale;
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    
    void Update()
    {
        
            
        

        Grounded();
        Move();
        Glide();
        Jump();
        CheckVerticalState();
        UpdateJumpVariales();
        DustO();
    }
    
    public void Move()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        float currentSpeed = isGround ? moveSpeed : airMoveSpeed;
        Vector2 movement = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
        anim.SetBool("isRunning", moveX != 0);
        if ((moveX > 0 && !isFacingRight) || (moveX < 0 && isFacingRight))
        {
            Flip();
        }  
    }
    public void DustO()
    {
        if (isGround && moveX != 0) 
        {
            dust.Play();
        }
        else
        {
            dust.Stop();
        }
    }
    void Glide()
    {
        if (jumping)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.gravityScale = glideGravityScale;
                anim.SetBool("Dive", true);
            }
            else
            {
                rb.gravityScale = fallGravityScale;
                anim.SetBool("Dive", false);
            }
        }
        if (!isGround && rb.linearVelocity.y < 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.gravityScale = glideGravityScale;
                anim.SetBool("Dive", true);
            }
            else
            {
                rb.gravityScale = fallGravityScale;
                anim.SetBool("Dive", false);
            }
        }
        else
        {
            rb.gravityScale = gravity;
            anim.SetBool("Dive", false);
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
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            jumping = false;
        }

        if (!jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCount > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumping = true;
                jumpBufferCounter = 0;
                isDoublejump = false;
                airjumpCount = 0;

                // ✨ Hiệu ứng DustBlast khi nhảy
                Instantiate(DustBlast, Foot.position, Quaternion.Euler(0, 0, 90));
            }
            else if (!Grounded() && airjumpCount < maxAirJump && Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumping = true;
                isDoublejump = true;
                airjumpCount++;

                // ✨ Hiệu ứng DustBlast khi double jump
                Instantiate(DustBlast, Foot.position, Quaternion.identity);
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
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", false);
                anim.SetBool("Dive", false);
                anim.SetBool("isRunning", false);
            }
            else if (linearVelocityY < -0.1f)
            {
                if (Input.GetKey(KeyCode.Space))
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
            anim.SetBool("Dive", false);
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            Debug.Log("🔄 Teleporting...");

            // Tắt di chuyển nhân vật
            playerController.enabled = false;
            rb.linearVelocity = Vector2.zero;

            // Đổi Rigidbody2D thành Kinematic
            rb.bodyType = RigidbodyType2D.Kinematic;

            // Đảm bảo Animator chạy dù Time.timeScale thay đổi
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;

            // Bắt đầu hiệu ứng chớp sáng
            StartCoroutine(TeleportAfterFlash());
        }
    }

    private IEnumerator TeleportAfterFlash()
    {
        // Chạy hiệu ứng flash
        yield return StartCoroutine(flashEffect.StartFlash());

        // Sau khi flash, di chuyển nhân vật
        transform.position = PortalFinish.transform.position;

        // Chạy animation
        anim.Play("Spin");
    }





}