using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject[] lockdoor; // Danh sách cửa của key này
    private Animator anim;
    private bool isUsed = false; // Trạng thái đã sử dụng

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isUsed)
        {
            isUsed = true; // Đánh dấu đã sử dụng để không bị kích hoạt nhiều lần
            anim.Play("KeyDisapear");
        }
    }

    public void DestroyAfterAnim()
    {
        Destroy(gameObject); // Hủy chìa khóa sau animation

        // Mở cửa chỉ của key này
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
