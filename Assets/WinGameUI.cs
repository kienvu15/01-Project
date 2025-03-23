using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinGameUI : MonoBehaviour
{
    public TextMeshProUGUI finishText;

    void Start()
    {
        int totalDeaths = PlayerPrefs.GetInt("DeathCount", 0);
        finishText.text = $"Die: {totalDeaths}";

        // Reset số lần chết khi bấm nút bất kỳ
        StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        ResetGame();
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteKey("DeathCount"); // Xóa số lần chết
        PlayerPrefs.Save();
        SceneManager.LoadScene(0); // Quay về màn hình menu hoặc màn 1
    }

    //IEnumerator

       
}
