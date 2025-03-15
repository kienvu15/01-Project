using UnityEngine;

public class SeeAwake : MonoBehaviour
{
    public Animator targetAnimator;
    public string animationName = "Aware"; // Tên animation trong Animator
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (targetAnimator != null)
            {
                targetAnimator.Play(animationName); // Phát animation trực tiếp
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Gọi hàm OnPlayerExitAware từ script Runenemy
            Runenemy enemyScript = targetAnimator.GetComponent<Runenemy>();
            if (enemyScript != null)
            {
                enemyScript.OnPlayerExitAware(); // Đánh dấu là Player đã rời khỏi
            }
        }
    }


}