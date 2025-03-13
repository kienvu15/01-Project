using UnityEngine;

public class PressureButton : MonoBehaviour
{
    private Animator anim;
    private int objectCount = 0; // Đếm số object đang chạm vào nút

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Untagged")) // Tránh kích hoạt bởi các object không mong muốn
        {
            objectCount++;
            anim.Play("ButtonPress"); // Chạy animation nhấn nút
            Debug.Log("🔴 Nút bị ấn xuống!");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Untagged"))
        {
            objectCount--;
            if (objectCount <= 0) // Khi tất cả object rời đi
            {
                anim.Play("Button"); // Quay về trạng thái ban đầu
                Debug.Log("🔵 Nút nhả ra!");
            }
        }
    }
}
