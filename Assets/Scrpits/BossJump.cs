using UnityEngine;

public class BossJump : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rb;
    PolygonCollider2D PolygonCollider2D;


    private bool isGround;
    private bool moveLeft;
    private Vector3 initialPosition; // Lưu vị trí ban đầu
    private bool initialState; // Lưu trạng thái ban đầu của canJumpeh

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float moveForce = 3f;
    public Transform Foot;
    public GameObject DustBlast;

    public AwareBoss AwareBoss;
    public GameObject Player;

    public bool die = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PolygonCollider2D = GetComponent<PolygonCollider2D>();

        initialPosition = transform.position; // Lưu vị trí ban đầu
        initialState = AwareBoss.canJumpeh;  // Lưu trạng thái ban đầu
    }

    void Update()
    {
        Grounded();
        CheckState();
    }

    private bool Grounded()
    {
        bool grounded = rb.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isGround = grounded;
        return grounded;
    }

    public void CheckState()
    {
        if (AwareBoss.canJumpeh)
        {
            moveLeft = Player.transform.position.x < rb.transform.position.x;
        }
    }

    // Hàm này sẽ được gọi từ Animation Event
    public void Jump()
    {
        if (rb == null || !isGround) return;
        if (AwareBoss.canJumpeh && isGround)
        {
            float horizontalMove = moveLeft ? -moveForce : moveForce;
            rb.linearVelocity = new Vector2(horizontalMove, jumpForce);
            Instantiate(DustBlast, Foot.position, Quaternion.Euler(0, 0, 90));
        }
    }

    // Hàm reset lại Boss khi người chơi respawn
    public void ResetBoss()
    {
        transform.position = initialPosition; 
        AwareBoss.canJumpeh = initialState;  
        rb.linearVelocity = Vector2.zero;
        PolygonCollider2D.enabled = true;

        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("New Animation");
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Player"; // Thay bằng tên sorting layer bạn muốn
            spriteRenderer.sortingOrder = 1; // Đặt order nhỏ hơn để bị đẩy ra sau
        }

        Debug.Log("Boss has been reset!");
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            anim.Play("Die");
            rb.linearVelocity = Vector2.zero;
            PolygonCollider2D.enabled = false;
            Debug.Log("Enemy Died in Water...");

            // Đổi Sorting Layer và Order in Layer
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Player"; // Thay bằng tên sorting layer bạn muốn
                spriteRenderer.sortingOrder = 3; // Đặt order nhỏ hơn để bị đẩy ra sau
            }

            anim.Play("Die");
            die = true;
            return;
        }
    }

}
