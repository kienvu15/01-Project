using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    //public PLboss[] bosses;
    //public BossJump BossJump;

    
    

    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    private float gravity;
    private bool isDead = false;
    [Space(5)]

    [Header("SoundEF")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip dieSound;
    [SerializeField] AudioClip moveSound; 
    [SerializeField] AudioClip nextLevel;
    [SerializeField] AudioClip app;
    [Space(5)]

    [Header("Movement")]
    [SerializeField] float moveSpeed = 3.5f;
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

    public TextMeshProUGUI deathCounterText;
    private int deathCount=0;
    public float appearForce;

    public int collectedItems = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravity = rb.gravityScale;
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();

        StartCoroutine(Appear());
    
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);
        UpdateDeathUI();
    }

    public IEnumerator Appear()
    {
        yield return new WaitForSeconds(0.3f);
        rb.AddForce(new Vector2(0, appearForce), ForceMode2D.Impulse);
        anim.Play("Appear");
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Fall", true);
    }

    public void SoundAppear()
    {
        audioSource.PlayOneShot(app);
    }

    void UpdateDeathUI()
    {
        if (deathCounterText != null)
        {
            deathCounterText.text = "" + deathCount;
        }
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
        
    }

    public float rayLength = 0.52f;
    private bool Grounded()
    {
        Vector2 rayOrigin = transform.position; // Điểm bắn tia
        Vector2 rayDirection = Vector2.down;   // Hướng xuống

        // Bắn tia Raycast kiểm tra cả "Ground" và "Ground2"
        int groundLayers = LayerMask.GetMask("Ground", "Ground2");
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, groundLayers);

        // Vẽ raycast để quan sát trong Scene
        Color rayColor = (hit.collider != null) ? Color.green : Color.red; // Xanh nếu chạm, đỏ nếu không chạm
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, rayColor);

        bool grounded = hit.collider != null;

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

        if (rb.linearVelocity.y < 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.gravityScale = glideGravityScale;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -1);
                anim.SetBool("Dive", true);
                Debug.Log("Guide while fall");
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
            else if (isGround == true && airjumpCount < maxAirJump && Input.GetKeyDown(KeyCode.Space))
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
            audioSource.PlayOneShot(nextLevel);
            anim.Play("Spin");
            playerController.enabled = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            StartCoroutine(TeleportAfterFlash());
        }
        if (collision.CompareTag("Enemies"))
        {
            StartCoroutine(flashEffect.StartFlash());
            audioSource.PlayOneShot(dieSound);
            Debug.Log("Player hit an enemy!");
            anim.SetBool("Die", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            isDead = true;

            Die();
        }
        if (collision.CompareTag("Item"))
        {
            collectedItems++;
            PlayerPrefs.SetInt("CollectedItems", collectedItems);
            PlayerPrefs.Save(); 
        }

    }
    
    private IEnumerator TeleportAfterFlash()
    {
        transform.position = PortalFinish.transform.position;
        yield return StartCoroutine(flashEffect.StartFlash());
        
        StartCoroutine(gridSpawner.SpawnGrid());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemies") && !isDead)
        {
            StartCoroutine(flashEffect.StartFlash());
            audioSource.PlayOneShot(dieSound);
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

        if (BoxManager.Instance != null)
            BoxManager.Instance.ResetAllBoxes();

        if (FallBrickManager.Instance != null)
            FallBrickManager.Instance.ResetAllBricks();

        if (LockDoorManager.Instance != null)
            LockDoorManager.Instance.ResetAllDoors();

        if (KeyManager.Instance != null)
            KeyManager.Instance.ResetAllKeys();

        if (SoftBlockManager.Instance != null)
            SoftBlockManager.Instance.ResetAllSoftBlocks();

        if (OpenManager.Instance != null)
            OpenManager.Instance.ResetAll();

        if (RunEnemyManager.Instance != null)
            RunEnemyManager.Instance.ResetAllEnemies();

        FallSpikeManager.Instance?.ResetAllSpikes();
        PLBossManager.Instance.ResetAllBosses();
    }
    public bool respawn = false;
    
    private IEnumerator Respawn()
    {
        respawn = true;

        GameObject tempGround = new GameObject("TempGround");
        tempGround.transform.position = respawnPoint.position + new Vector3(0, -1, 0);
        BoxCollider2D tempCollider = tempGround.AddComponent<BoxCollider2D>();
        tempCollider.isTrigger = false; // Đảm bảo nền có va chạm

        yield return new WaitForSeconds(0.5f); // Đợi một chút để nhân vật ổn định
        Destroy(tempGround); // Xóa nền tạm thời
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        
        deathCount++;
        PlayerPrefs.SetInt("DeathCount", deathCount);
        PlayerPrefs.Save();
        UpdateDeathUI();

        // Đặt lại vị trí camera
        Camera.main.transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y, Camera.main.transform.position.z);

        yield return new WaitForSeconds(3f); // Đợi hiệu ứng chết

        // Đặt nhân vật về vị trí respawn trước khi bật lại vật lý
        transform.position = respawnPoint.position;

        // Reset Rigidbody và Collider
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero; // Reset tốc độ để tránh rơi tiếp
        rb.angularVelocity = 0f;
        myBodyCollider.enabled = true;
        yield return new WaitForSeconds(0.1f); // Đợi một frame để đảm bảo collider đã bật
        
        // Đổi về Dynamic sau khi đã ổn định vị trí
        rb.bodyType = RigidbodyType2D.Dynamic;

        Debug.Log("🔄 Respawning...");

        // Reset trạng thái nhân vật
        isDead = false;
        anim.SetBool("Die", false);
        anim.Play("Idle");
        audioSource.PlayOneShot(app);
        yield return new WaitForSeconds(0.2f); // Đợi một chút trước khi reset tất cả

        
        
        // Kiểm tra null trước khi gọi Reset



        


        //if (BossJump != null)
        //    BossJump.ResetBoss();



    }




}