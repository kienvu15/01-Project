using System.Collections;
using UnityEngine;

public class FallSpike : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;

    [Header("VFX")]
    public GameObject DustBlast;
    public GameObject DustBlast2;
    public Transform point;
    public Transform point2;
    public GameObject aware;

    public AudioSource audioSource;
    [SerializeField] AudioClip Fall;
    [SerializeField] AudioClip Land;

    // Lưu trạng thái ban đầu
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private RigidbodyType2D initialBodyType;

    private void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        // Kiểm tra gravity scale
        if (body.gravityScale == 0)
            body.gravityScale = 1f;

        // Lưu trạng thái ban đầu
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialBodyType = body.bodyType;

        FallSpikeManager.Instance?.RegisterFallSpike(this);
    }

    public void OnComlpete()
    {
        animator.Play("Spike");
        body.bodyType = RigidbodyType2D.Dynamic;
        body.gravityScale = 1f;

        if (aware != null)
        {
            aware.SetActive(true); // Không tắt Aware
        }

        Instantiate(DustBlast, body.position, Quaternion.identity);
        Instantiate(DustBlast2, point.position, Quaternion.identity);
        Instantiate(DustBlast2, point2.position, Quaternion.identity);
    }

    public void Osnund()
    {
        audioSource.PlayOneShot(Fall);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
            body.bodyType = RigidbodyType2D.Static;
            animator.enabled = false;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play("shake4");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            audioSource.PlayOneShot(Land);
        }
    }
    // 🛠 Reset lại FallBrick khi Player Respawn
    public void ResetBrick()
    {
        // Đặt lại vị trí, xoay và trạng thái Rigidbody
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        body.bodyType = initialBodyType;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;

        // Bật lại Animator nhưng đảm bảo nó không chạy animation trước đó
        animator.enabled = true;
        animator.Play("Spike", 0, 0f);

        // Reset Collider nếu cần
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
            Invoke(nameof(ReenableCollider), 0.1f);
        }

        // Reset trạng thái của Aware để không bị kích hoạt ngay
        if (aware != null)
        {
            aware.SetActive(true);

            Aware awareScript = aware.GetComponent<Aware>();
            if (awareScript != null)
            {
                awareScript.ResetAware();
            }
        }

        gameObject.SetActive(true);
    }

    private void ReenableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }
    }




}
