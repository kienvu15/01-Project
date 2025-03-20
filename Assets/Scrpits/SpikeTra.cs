using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Nếu người chơi chạm vào bẫy
        {
            Debug.Log("Player chạm vào vùng bẫy! Game Over!");
            Time.timeScale = 0; // Dừng game
        }
    }
}
