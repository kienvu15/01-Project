using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain2 : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed = 2f;
    private float originMoveSpeed;

    public Transform[] movePoints; // Danh sách các điểm di chuyển
    private int currentTargetIndex = 0; // Vị trí hiện tại trong danh sách
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originMoveSpeed = moveSpeed;

        if (movePoints.Length > 0)
            transform.position = movePoints[0].position; // Đặt vị trí ban đầu tại điểm đầu tiên
    }

    void Update()
    {
        if (movePoints.Length > 1)
        {
            MoveTrapToTarget();
            FlipDirectionIfNeeded();
        }
    }

    private void MoveTrapToTarget()
    {
        Transform currentTarget = movePoints[currentTargetIndex];
        rb.position = Vector2.MoveTowards(rb.position, currentTarget.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(rb.position, currentTarget.position) < 0.1f)
        {
            currentTargetIndex = (currentTargetIndex + 1) % movePoints.Length; // Chuyển sang điểm tiếp theo, lặp lại khi đến điểm cuối
        }
    }

    private void FlipDirectionIfNeeded()
    {
        Transform nextTarget = movePoints[currentTargetIndex];

        if ((nextTarget.position.x < transform.position.x && isFacingRight) ||
            (nextTarget.position.x > transform.position.x && !isFacingRight))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
