using UnityEngine;

public class HoldPressureButton : MonoBehaviour
{
    private Animator anim;
    private int objectCount = 0; // Đếm số object đang chạm vào nút
    public GameObject[] lockdoor; // Danh sách cửa của key này
    private bool isUsed = false; // Trạng thái đã sử dụng

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
            isUsed = true;
            foreach (GameObject door in lockdoor)
            {
                LockDoor lockDoorScript = door.GetComponent<LockDoor>();
                if (lockDoorScript != null)
                {
                    lockDoorScript.PlayAnimation();
                }
            }
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
                isUsed = false;
                foreach (GameObject door in lockdoor)
                {
                    LockDoor lockDoorScript = door.GetComponent<LockDoor>();
                    if (lockDoorScript != null)
                    {
                        lockDoorScript.Actice();
                    }
                }
            }
        }
    }
}
