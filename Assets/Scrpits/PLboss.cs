using UnityEngine;

public class PLboss : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    public GameObject dust;
    public GameObject dust2;
    public GameObject point1;
    public GameObject point2;

    private float count = 0;

    // Lưu trạng thái ban đầu
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private RigidbodyType2D initialBodyType;

    public AudioSource audioSource;
    [SerializeField] AudioClip Clicked;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Lưu trạng thái ban đầu
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialBodyType = rb.bodyType;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            count++;
            anim.Play("Week");
            audioSource.PlayOneShot(Clicked);
            Instantiate(dust, point1.transform.position, Quaternion.identity);
            Instantiate(dust2, point2.transform.position, Quaternion.identity);

            if (count == 2)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // Đổi Rigidbody thành Dynamic
                boxCollider.gameObject.SetActive(false);
                Invoke(nameof(DisableBoss), 2f); // Sau 2 giây, tắt GameObject
            }
        }
    }

    void DisableBoss()
    {
        gameObject.SetActive(false);
    }

    // Hàm reset trạng thái khi Player respawn
    public void ResetBoss()
    {
        boxCollider.gameObject.SetActive(true);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.bodyType = initialBodyType;
        count = 0;
        gameObject.SetActive(true);
        anim.Play("Idle");
    }
}
