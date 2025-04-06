using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinGameUI : MonoBehaviour
{
    public TextMeshProUGUI finishText;
    public TextMeshProUGUI itemCounterText;
    public float allCrown;

    void Start()
    {
        int totalDeaths = PlayerPrefs.GetInt("DeathCount", 0);
        finishText.text = $"Die: {totalDeaths}";

        int collectedItems = PlayerPrefs.GetInt("CollectedItems", 0); // Lấy số item đã ăn
        itemCounterText.text = $"{collectedItems}/{allCrown}";
    }

    // Hàm này sẽ được gọi khi Button Reset được bấm
    public void OnResetButtonClicked()
    {
        
            Debug.Log("Nút được bấm!");
            ResetGame();
        

    }

    private void ResetGame()
    {
        PlayerPrefs.DeleteKey("DeathCount"); // Xóa số lần chết
        PlayerPrefs.SetInt("CollectedItems", 0); // Reset về 0
        PlayerPrefs.Save();

        SceneManager.LoadScene("Open"); // Quay về màn hình menu hoặc màn 1
    }
}
