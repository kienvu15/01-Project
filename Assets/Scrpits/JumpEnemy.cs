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

        if (isGround && !canJump)
        {
            canJump = true;
        }
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
        if (rb == null || isGround == false || canJump == false) return;

        float horizontalMove = moveLeft ? -moveForce : moveForce;
        rb.linearVelocity = new Vector2(horizontalMove, jumpForce);
        Instantiate(DustBlast, Foot.position, Quaternion.Euler(0, 0, 90));

        moveLeft = !moveLeft;
        canJump = false;
    }

}
