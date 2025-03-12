using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
    public GameObject levelButtonPrefab;  // Prefab button level
    public List<Transform> levelPanels;   // Danh sách các panel chứa button
    public int levelsPerPanel = 15;       // Số level mỗi panel
    public int levelsPerRow = 5;          // Số button mỗi hàng

    private void Start()
    {
        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        int totalLevels = levelPanels.Count * levelsPerPanel; // Tổng số level

        for (int i = 0; i < totalLevels; i++)
        {
            int levelNumber = i + 1;  // Level thực tế (1-45...)
            int panelIndex = i / levelsPerPanel; // Xác định panel chứa level này
            if (panelIndex >= levelPanels.Count) break; // Nếu vượt quá số panel, dừng lại

            // Xác định vị trí trong grid
            int row = (i % levelsPerPanel) / levelsPerRow;  // Chỉ số hàng
            int col = (i % levelsPerPanel) % levelsPerRow;  // Chỉ số cột

            

            // Tạo button trong panel tương ứng
            GameObject newButton = Instantiate(levelButtonPrefab, levelPanels[panelIndex]);
            newButton.name = "Level " + levelNumber;

            // Cập nhật text trên button
            TextMeshProUGUI tmpText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
                tmpText.text = levelNumber.ToString();
            else
                Debug.LogError("⚠️ Không tìm thấy TextMeshPro trong prefab button!");

            // Đặt sự kiện onClick cho button
            int levelIndex = levelNumber;
            newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));

            // Đặt lại scale để tránh lỗi hiển thị
            newButton.transform.localScale = Vector3.one;
        }
    }

    void LoadLevel(int level)
    {
        string sceneName = "level" + level.ToString("00");  // Ví dụ: level01, level02...

        Debug.Log("🔍 Loading Level: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
