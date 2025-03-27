using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject[] lockdoor; // Danh sách cửa mà Key này mở
    private Animator anim;
    private bool isUsed = false; // Trạng thái đã sử dụng

    void Start()
    {
        anim = GetComponent<Animator>();
        KeyManager.Instance.RegisterKey(this); // Đăng ký vào Manager
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
        gameObject.SetActive(false); // Ẩn chìa khóa sau animation

        // Mở cửa tương ứng với Key này
        foreach (GameObject door in lockdoor)
        {
            LockDoor lockDoorScript = door.GetComponent<LockDoor>();
            if (lockDoorScript != null)
            {
                lockDoorScript.PlayAnimation();
            }
        }
    }

    public void ResetKey()
    {
        isUsed = false; // Reset trạng thái để có thể dùng lại
        gameObject.SetActive(true); // Bật lại Key
    }
}
