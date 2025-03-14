using UnityEngine;

public class JumpEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool moveLeft = true; // Lần đầu tiên sẽ nhảy sang trái
    private bool isGround;
    private bool canJump = true;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float moveForce = 3f;
    public Transform Foot;
    public GameObject DustBlast;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Grounded();
    }
    private bool Grounded()
    {
        bool grounded = rb.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (grounded)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        return grounded;
    }

    

    // Hàm này sẽ được gọi từ Animation Event
    public void Jump()
    {
        if (rb == null || isGround==false) return;

        // Xác định hướng nhảy
        float horizontalMove = moveLeft ? -moveForce : moveForce;

        // Thêm lực nhảy
        rb.linearVelocity = new Vector2(horizontalMove, jumpForce);
        Instantiate(DustBlast, Foot.position, Quaternion.Euler(0, 0, 90));

        // Đảo hướng cho lần nhảy tiếp theo
        moveLeft = !moveLeft;

        // Không cho phép nhảy tiếp cho đến khi chạm đất lại
        canJump = false;
    }
}
