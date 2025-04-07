using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public FlashEffect flashEffect;
    public GridSpawner gridSpawner;

    public void PlayGame()
    {
        StartCoroutine(TeleportAfterFlash());
        SceneManager.LoadScene("Stagelevel");
    }

    private IEnumerator TeleportAfterFlash()
    {
        yield return StartCoroutine(flashEffect.StartFlash());   
        StartCoroutine(gridSpawner.SpawnGrid());
    }

    public GameObject canvasToShow; // Gán Canvas hoặc Panel ở đây trong Inspector

    public void ShowCanvas()
    {
        if (canvasToShow != null)
        {
            canvasToShow.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Chưa gán canvasToShow!");
        }
    }

    public void HideCanvas()
    {
        canvasToShow.SetActive(false); 
    }


}
