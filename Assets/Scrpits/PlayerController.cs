using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    public AudioSource audioSource;
    private PlayerController playerController;
    public FlashEffect flashEffect;
    public GridSpawner gridSpawner;
    public Transform respawnPoint;
    public SoftBlock[] softBlocks;
    public Runenemy[] enemies;
    public FallBrick FallBrick;

    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private float gravity;
    private bool isDead = false;
    [Space(5)]

    [Header("SoundEF")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip moveSound;
    [Space(5)]

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMoveSpeed = 5f;
    private float moveX;
    private bool isFacingRight = true;
    public float footstepInterval = 0.3f; // Thời gian giữa mỗi bước chân
    private float footstepTimer;
    [Space(5)]

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
        myBodyCollider = GetComponent<BoxCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravity = rb.gravityScale;
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();

        rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
        anim.Play("Appear");
        StartCoroutine(flashEffect.StartFlash());
    }
    public void WaitForAppearAnimation()
    {
        
        anim.SetBool("Fall",true);
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
        if (isDead)
        {
            ResetSoftBlocks();
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
    public void Move()
    {
        if (isDead) return;

        moveX = Input.GetAxisRaw("Horizontal");
        float currentSpeed = isGround ? moveSpeed : airMoveSpeed;
        Vector2 movement = new Vector2(moveX * currentSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;

        anim.SetBool("isRunning", moveX != 0);

        if ((moveX > 0 && !isFacingRight) || (moveX < 0 && isFacingRight))
        {
            Flip();
        }

        // Giảm footstepTimer theo thời gian
        footstepTimer -= Time.deltaTime;

        // Kiểm tra nếu đang chạy trên mặt đất và timer đã hết
        if (isGround && moveX != 0 && footstepTimer <= 0)
        {
            audioSource.PlayOneShot(moveSound);
            footstepTimer = footstepInterval; // Reset timer
        }
    }
    public void DustO()
    {
        if (isDead == true) return;
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
        if (isDead == true) return;
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
        if (isDead == true) return;
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
                Instantiate(DustBlast, Foot.position, Quaternion.Euler(0, 0, 90));
                audioSource.PlayOneShot(jumpSound);
            }
            else if (!Grounded() && airjumpCount < maxAirJump && Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumping = true;
                isDoublejump = true;
                airjumpCount++;
                Instantiate(DustBlast, Foot.position, Quaternion.identity);
            }
        }
    }

    public void CheckVerticalState()
    {
        if (isDead == true) return;
        float linearVelocityY = rb.linearVelocity.y;
        if(isDead == true)
        {
            anim.SetBool("Die", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            anim.SetBool("Dive", false);
            anim.SetBool("isRunning", false);
        }
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
                    if (isDead == true)
                    {
                        anim.SetBool("Fall", false);
                        anim.SetBool("Die", true);
                    }
                }
            }
        }
        else
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            anim.SetBool("Dive", false);
        }
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
            playerController.enabled = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            StartCoroutine(TeleportAfterFlash());
        }
        if (collision.CompareTag("Enemies"))
        {
            StartCoroutine(flashEffect.StartFlash());
            Debug.Log("Player hit an enemy!");
            anim.SetBool("Die", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            isDead = true;

            Die();
        }
        
    }
    
    private IEnumerator TeleportAfterFlash()
    {
        transform.position = PortalFinish.transform.position;
        yield return StartCoroutine(flashEffect.StartFlash());
        anim.Play("Spin");
        StartCoroutine(gridSpawner.SpawnGrid());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemies") && !isDead)
        {
            StartCoroutine(flashEffect.StartFlash());
            Debug.Log("Player hit an enemy!");
            anim.SetBool("Die", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            isDead = true;
            
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("💀 Player Died!");
        rb.AddForce(new Vector2(0, 15f), ForceMode2D.Impulse);
        myBodyCollider.enabled = false;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        Camera.main.transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, Camera.main.transform.position.z);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(3f);

        Debug.Log("🔄 Respawning...");
        myBodyCollider.enabled = true;
        

        // Đưa nhân vật về vị trí checkpoint
        transform.position = respawnPoint.position;

        // Reset trạng thái nhân vật
        isDead = false;
        anim.SetBool("Die", false);
        anim.Play("Idle");

        // Reset các gạch rơi
        FallBrick.ResetAllBricks();

        // Reset tất cả enemy về vị trí và trạng thái ban đầu
        foreach (Runenemy enemy in enemies)
        {
            enemy.ResetEnemy();
        }
    }


    void ResetSoftBlocks()
    {
        foreach (SoftBlock block in softBlocks)
        {
            block.ResetPlatform();
        }
    }



}