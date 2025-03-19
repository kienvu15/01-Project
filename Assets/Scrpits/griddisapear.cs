using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class griddisapear : MonoBehaviour
{
    public GameObject panel;
    public GameObject objectPrefab; //OpenTransition
    public int columns = 15;
    public int rows = 7;
    public float spacing = 1.45f;
    public float rowDelay = 0.16f;
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

        

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject obj = gridObjects[y, x];

                // Bật animation cho từng object
                Animator anim = obj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.Play(animationName);
                }
            }
            yield return new WaitForSeconds(rowDelay);
        }
        yield return StartCoroutine(flashEffect.StartFlash());

        canPlay = true;
    }
}