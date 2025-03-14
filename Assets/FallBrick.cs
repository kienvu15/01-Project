using UnityEngine;
using System.Collections.Generic;

public class FallBrick : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;

    [Header("VFX")]
    public GameObject DustBlast;
    public GameObject DustBlast2;
    public Transform point;
    public Transform point2;
    public GameObject aware;

    // Lưu trạng thái ban đầu
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private RigidbodyType2D initialBodyType;

    // Danh sách chứa tất cả FallBrick
    private static List<FallBrick> allBricks = new List<FallBrick>();

    private void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        // Lưu trạng thái ban đầu
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialBodyType = body.bodyType;

        // Đăng ký vào danh sách quản lý
        allBricks.Add(this);
    }

    public void OnComlpete()
    {
        animator.Play("IdleBrick");
        body.bodyType = RigidbodyType2D.Dynamic;
        aware.SetActive(false);
        Instantiate(DustBlast, body.position, Quaternion.identity);
        Instantiate(DustBlast2, point.position, Quaternion.identity);
        Instantiate(DustBlast2, point2.position, Quaternion.identity);
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            body.bodyType = RigidbodyType2D.Static;
            animator.enabled = false;
        }
    }


    public void ResetBrick()
    {
        // Reset vị trí và trạng thái
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        body.bodyType = initialBodyType;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        aware.SetActive (true);
        animator.enabled = true;
        gameObject.SetActive(true);
    }

    // Reset tất cả các FallBrick
    public static void ResetAllBricks()
    {
        foreach (var brick in allBricks)
        {
            brick.ResetBrick();
        }
    }
}
