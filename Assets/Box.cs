using UnityEngine;

public class Box : MonoBehaviour
{
    private Vector3 initialPosition; // Lưu vị trí ban đầu
    private Rigidbody2D rb;

    void Start()
    {
        initialPosition = transform.position; // Lưu vị trí ban đầu khi game bắt đầu
        rb = GetComponent<Rigidbody2D>();

        BoxManager.Instance.RegisterBox(this); // Đăng ký vào BoxManager
    }

    public void ResetBox()
    {
        transform.position = initialPosition; // Đưa về vị trí ban đầu
        rb.linearVelocity = Vector2.zero; // Dừng di chuyển
        rb.angularVelocity = 0f; // Dừng xoay nếu có
    }
}
