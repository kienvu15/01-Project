using UnityEngine;

public class Crow : MonoBehaviour
{
    Animator animator;
    CircleCollider2D CircleCollider2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CircleCollider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.Play("crow2");
        }
    }
    public void CollDonr()
    {
        CircleCollider2D.enabled = false;
    }
    public void DoneAnimation()
    {
        Destroy(gameObject);
    }
}
