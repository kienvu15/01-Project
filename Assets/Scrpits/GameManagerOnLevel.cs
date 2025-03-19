using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerOnLevel : MonoBehaviour
{
    public GameObject pausePanel; // Panel hiển thị menu pause
    public Button pauseButton;    // Nút pause

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // Ẩn panel ban đầu
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Dừng game
        pausePanel.SetActive(true); // Hiện menu pause
        pauseButton.gameObject.SetActive(false); // Ẩn nút pause
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;  // Đảm bảo game không bị pause khi restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Load lại scene hiện tại
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Tiếp tục game
        pausePanel.SetActive(false); // Ẩn menu pause
        pauseButton.gameObject.SetActive(true); // Hiện lại nút pause
    }
    public void GoToMenu()
    {
        Time.timeScale = 1f;  // Reset lại thời gian game trước khi chuyển Scene
        SceneManager.LoadScene("Open"); // Chuyển đến Scene "Open"
    }
}
