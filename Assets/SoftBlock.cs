using System.Collections;
using UnityEngine;

public class SoftBlock : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector3 initialPosition; // Lưu vị trí ban đầu
    private bool isActivated = false;

    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float destroyDelay = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position; // Lưu vị trí ban đầu
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
            anim.Play("Zinggel_Sofr");
        }
    }
    
    public void DeActive()
    {
        boxCollider.enabled = false;
    }

    private void On2Destroy()
    {
        gameObject.SetActive(false); // Ẩn thay vì destroy
    }

    // Hàm Reset Platform
    public void ResetPlatform()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
        boxCollider.enabled = true;
        isActivated = false;
        anim.Play("Idle"); // Hoặc trạng thái mặc định
    }
}
