using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PanelSwitcher : MonoBehaviour
{
    public CanvasGroup[] pages; // Danh sách các Page
    public Button nextButton, previousButton;
    private int currentIndex = 0;

    public FlashEffect flashEffect;

    public Canvas Stageseleect;
    public GameObject pageContainer;
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    private void Start()
    {
        UpdatePanels();
    }
    public void Return()
    {
        SceneManager.LoadScene("Open");
    }
    public void Reutrun2Stage()
    {
        pageContainer.SetActive(false);
        Stageseleect.gameObject.SetActive(true);
    }
    public void NextPage()
    {
        if (currentIndex < pages.Length - 1)
        {
            currentIndex++;
            UpdatePanels();
        }
    }

    public void PreviousPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePanels();
        }
    }

    private void UpdatePanels()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].gameObject.SetActive(i == currentIndex);
            pages[i].alpha = (i == currentIndex) ? 1 : 0;
            pages[i].interactable = (i == currentIndex);
        }

        // Cập nhật trạng thái nút
        previousButton.gameObject.SetActive(currentIndex > 0);
        nextButton.gameObject.SetActive(currentIndex < pages.Length - 1);
    }

    public void PlayWorld1()
    {
        Stageseleect.gameObject.SetActive(false);
        pageContainer.SetActive(true);
        page1.SetActive(true);
    }
    public void PlayWorld2()
    {
        Stageseleect.gameObject.SetActive(false);
        pageContainer.SetActive(true);
        page2.SetActive(true);
    }
    public void PlayWorld3()
    {
        Stageseleect.gameObject.SetActive(false);
        pageContainer.SetActive(true);
        page3.SetActive(true);
    }

}