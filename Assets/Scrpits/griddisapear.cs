using System.Collections;
using UnityEngine;

public class griddisapear : MonoBehaviour
{
    public GameObject panel;
    public GameObject objectPrefab; // OpenTransition
    public int columns = 15;
    public int rows = 7;
    public float spacing = 1.45f;
    public Transform spawnOrigin;
    public FlashEffect flashEffect;
    public string animationName = "OpenTransition"; // Tên animation trong Animator

    private GameObject[,] gridObjects;
    public static bool canPlay = false;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startSound;

    void Start()
    {
        SpawnGridInstantly();
        StartCoroutine(PlayGridAnimation());

        canPlay = false;
    }

    void SpawnGridInstantly()
    {
        gridObjects = new GameObject[rows, columns];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 position = spawnOrigin.position + new Vector3(x * spacing, y * -spacing, 0);
                GameObject obj = Instantiate(objectPrefab, position, Quaternion.identity);
                obj.SetActive(true); // Hiện tất cả object ngay lập tức
                gridObjects[y, x] = obj;
            }
        }
    }

    IEnumerator PlayGridAnimation()
    {
        if (audioSource != null && startSound != null)
        {
            audioSource.PlayOneShot(startSound);
        }

        float animationDuration = 0f;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject obj = gridObjects[y, x];

                Animator anim = obj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.Play(animationName);

                    // Lấy thời gian animation dài nhất
                    AnimationClip clip = anim.runtimeAnimatorController.animationClips[0];
                    if (clip.length > animationDuration)
                    {
                        animationDuration = clip.length;
                    }
                }
            }
        }

        // Đợi cho đến khi tất cả animation kết thúc
        yield return new WaitForSeconds(animationDuration);

        // Kích hoạt hiệu ứng Flash ngay lập tức khi animation kết thúc
        yield return StartCoroutine(flashEffect.StartFlash());

        // XÓA TẤT CẢ OBJECT SAU KHI FLASH KẾT THÚC
        ClearGrid();

        canPlay = true;
    }

    void ClearGrid()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (gridObjects[y, x] != null)
                {
                    Destroy(gridObjects[y, x]);
                }
            }
        }
    }
}
