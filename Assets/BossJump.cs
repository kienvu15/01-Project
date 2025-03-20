using UnityEngine;

public class BossJump : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private bool isGround;
    private bool canJump = true;
    private bool moveLeft;
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
    }
    void Update()
    {
        Grounded();
        CheckState();
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


    public void CheckState()
    {
        if (AwareBoss.canJumpeh == true)
        {
            if (Player.transform.position.x < rb.transform.position.x)
            {
                moveLeft = true;
            }
            else
            {
                moveLeft = false;
            }
        }
    }
    // Hàm này sẽ được gọi từ Animation Event
    public void Jump()
    {
        if (rb == null || isGround == false) return;
        if (AwareBoss.canJumpeh == true && isGround == true) 
        {
            // Xác định hướng nhảy
            float horizontalMove = moveLeft ? -moveForce : moveForce;

            // Thêm lực nhảy
            rb.linearVelocity = new Vector2(horizontalMove, jumpForce);
            Instantiate(DustBlast, Foot.position, Quaternion.Euler(0, 0, 90));


            
        }


            
    }
}
