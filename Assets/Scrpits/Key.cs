using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject[] lockdoor; // Danh sách cửa mà Key này mở
    private Animator anim;
    private bool isUsed = false; // Trạng thái đã sử dụng
    public float delayBetweenDoors = 1f; // Độ trễ giữa mỗi lần mở cửa

    void Start()
    {
        anim = GetComponent<Animator>();
        KeyManager.Instance.RegisterKey(this); // Đăng ký vào Manager
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isUsed)
        {
            isUsed = true;
            anim.Play("KeyDisapear");

            // Gọi Coroutine từ KeyManager
            KeyManager.Instance.StartOpeningDoors(lockdoor);
        }

    }

    public void DestroyAfterAnim()
    {
        StartCoroutine(OpenDoorsInOrder()); // Chạy Coroutine trước
        gameObject.SetActive(false); // Sau đó mới ẩn Key
    }


    private IEnumerator OpenDoorsInOrder()
    {
        foreach (GameObject door in lockdoor)
        {
            if (door != null)
            {
                LockDoor lockDoorScript = door.GetComponent<LockDoor>();
                if (lockDoorScript != null)
                {
                    lockDoorScript.PlayAnimation();
                }
            }
            yield return new WaitForSeconds(delayBetweenDoors); // Chờ trước khi mở cửa tiếp theo
        }
    }

    public void ResetKey()
    {
        isUsed = false; // Reset trạng thái để có thể dùng lại
        gameObject.SetActive(true); // Bật lại Key
    }
}
