using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashEffect : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.2f; // Thời gian chớp sáng

    private void Start()
    {
        if (flashImage != null)
            flashImage.enabled = false;
    }

    public IEnumerator StartFlash()
    {
        if (flashImage == null) yield break;

        flashImage.enabled = true;
        flashImage.color = new Color(1, 1, 1, 1); // Chớp trắng toàn màn hình

        yield return new WaitForSeconds(flashDuration);

        // Làm mờ dần hiệu ứng
        float fadeTime = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            flashImage.color = new Color(1, 1, 1, 1 - (elapsedTime / fadeTime));
            yield return null;
        }

        flashImage.enabled = false;
    }
}
