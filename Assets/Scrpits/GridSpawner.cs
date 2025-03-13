using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridSpawner : MonoBehaviour
{
    public GameObject panel;
    public GameObject objectPrefab;  // Prefab của object có animation
    public int columns = 15;  // Số cột (tùy chỉnh theo màn hình)
    public int rows = 7;  // Số hàng
    public float spacing = 1.45f;  // Khoảng cách giữa các object
    public float rowDelay = 0.16f; // Thời gian delay giữa các hàng
    public Transform spawnOrigin;  // Điểm gốc để spawn các object
    public FlashEffect flashEffect;
    public string nextSceneName;
    
    void Start()
    {
        
    }

    public IEnumerator SpawnGrid()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 position = spawnOrigin.position + new Vector3(x * spacing, y * -spacing, 0);

                GameObject obj = Instantiate(objectPrefab, position, Quaternion.identity);

                Animator anim = obj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.Play("AnimationName");  // Chạy animation
                }
            }
            yield return new WaitForSeconds(rowDelay); // Delay giữa từng hàng
        }
        yield return StartCoroutine(flashEffect.StartFlash());

        // Đổi sang scene mới
        SceneManager.LoadScene(nextSceneName);
    }
}
