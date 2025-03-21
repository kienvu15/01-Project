using UnityEngine;

public class AwareBoss : MonoBehaviour
{
    public bool canJumpeh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canJumpeh = true;
        }
    }
}
