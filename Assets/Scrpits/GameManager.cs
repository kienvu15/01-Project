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

}
