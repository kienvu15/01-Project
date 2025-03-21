using UnityEngine;

public class BossJump : MonoBehaviour
{
    private Rigidbody2D rb;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        transform.position = initialPosition; // Reset vị trí về ban đầu
        AwareBoss.canJumpeh = initialState;  // Reset trạng thái
        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động
    }
}
