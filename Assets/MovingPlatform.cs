using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA, pointB; // 2 điểm platform sẽ di chuyển giữa
    public float moveSpeed = 2f; // Tốc độ di chuyển

    private Transform target; // Mục tiêu hiện tại

    void Start()
    {
        target = pointA; // Bắt đầu di chuyển từ điểm A
    }

    void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);

        // Nếu đến gần target, đổi hướng di chuyển
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // Gán Player làm con của platform
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // Hủy quan hệ cha-con khi Player rời khỏi
        }
    }
}
