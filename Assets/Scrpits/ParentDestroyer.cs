using UnityEngine;

public class ParentDestroyer : MonoBehaviour
{
    void Update()
    {
        // Kiểm tra nếu không còn child nào
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
