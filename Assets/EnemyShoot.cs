using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public bool isFacingRight = true; // Hướng của Enemy (true = phải, false = trái)
    public GameObject bulletPrefab; // Prefab viên đạn
    public Transform firePoint; // Điểm bắn đạn
    public float bulletSpeed = 5f; // Tốc độ viên đạn
    public Animator animator; // Tham chiếu đến Animator

    public float WaitIdle = 2f;
    public float BetweenShot = 2f;
    private void Start()
    {
        StartCoroutine(StartShootingLoop());
    }

    IEnumerator StartShootingLoop()
    {
        while (true) // Lặp vô hạn
        {
            yield return new WaitForSeconds(WaitIdle); // Chờ 2 giây ở trạng thái "Idle"
            animator.SetBool("Shoot", true); // Chạy animation bắn
            yield return new WaitForSeconds(BetweenShot);
        }
    }

    public void OnComplete()
    {
        animator.SetBool("Shoot", false);
    }
    // Hàm này sẽ được gọi bởi Animation Event
    public void ShootBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Tạo viên đạn
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Xác định hướng bắn
        float direction = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * bulletSpeed, 0f); // Bắn theo hướng enemy đang nhìn

        // Nếu đang quay trái, thì flip để quay về đúng hướng trước khi bắn
        if (!isFacingRight)
        {
            Flip();
        }
    }

    // Hàm lật hướng Enemy
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
