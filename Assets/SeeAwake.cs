using UnityEngine;

public class SeeAwake : MonoBehaviour
{
    public Animator targetAnimator;
    public string animationName = "Aware"; // Tên animation trong Animator
    public static int awareCount = 0; // Đếm số vùng Aware mà Player đang ở

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            awareCount++;
            Debug.Log("Player entered SeeAwake! Total Aware Zones: " + awareCount);

            if (targetAnimator != null)
            {
                targetAnimator.Play(animationName);
                Debug.Log("Playing Aware animation!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            awareCount = Mathf.Max(0, awareCount - 1); // Giảm nhưng không xuống dưới 0
            Debug.Log("Player exited Aware zone. Remaining Aware Zones: " + awareCount);

            if (awareCount == 0)
            {
                Runenemy enemyScript = GetComponentInParent<Runenemy>();
                if (enemyScript != null)
                {
                    enemyScript.OnPlayerExitAware();
                    Debug.Log("All Aware zones cleared, stopping enemy...");
                }
            }
        }
    }
}
