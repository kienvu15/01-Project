using UnityEngine;

public class SoftBlock : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector3 initialPosition; // Lưu vị trí ban đầu
    private bool isActivated = false;

    [SerializeField] private float destroyDelay = 2f;

    public AudioSource audioSource;
    [SerializeField] AudioClip breakSound;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position; // Lưu vị trí ban đầu

        SoftBlockManager.Instance.RegisterSoftBlock(this); // Đăng ký vào Manager
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            anim.Play("Zinggel_Sofr");
        }
    }

    public void ONsounf()
    {
        audioSource.PlayOneShot(breakSound);

    }
    public void DeActive()
    {
        boxCollider.enabled = false;
    }

    private void On2Destroy()
    {
        gameObject.SetActive(false); // Ẩn thay vì destroy
    }

    // 🛠 Reset lại SoftBlock khi Player Respawn
    public void ResetSoftBlock()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        boxCollider.enabled = true;
        isActivated = false;
        anim.Play("Idle"); // Reset animation về trạng thái mặc định
    }
}
