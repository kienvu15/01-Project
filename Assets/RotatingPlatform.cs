using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public Transform[] platforms; // Danh sách các platform (4 cái)
    public float radius = 2f; // Bán kính quay
    public float speed = 50f; // Tốc độ quay

    private float angleOffset; // Góc ban đầu của từng platform

    void Start()
    {
        angleOffset = 360f / platforms.Length; // Chia đều 4 góc
    }

    void Update()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            float angle = (Time.time * speed) + (angleOffset * i); // Tính góc quay
            float radian = angle * Mathf.Deg2Rad;

            // Tính vị trí mới theo quỹ đạo tròn
            Vector3 newPosition = new Vector3(
                transform.position.x + Mathf.Cos(radian) * radius,
                transform.position.y + Mathf.Sin(radian) * radius,
                platforms[i].position.z
            );

            platforms[i].position = newPosition;

            // Giữ nguyên góc Z
            platforms[i].rotation = Quaternion.identity;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius); // Vẽ đường tròn bán kính radius

        if (platforms != null && platforms.Length > 0)
        {
            Gizmos.color = Color.red;
            foreach (Transform platform in platforms)
            {
                Gizmos.DrawSphere(platform.position, 0.1f); // Vẽ chấm đỏ ở vị trí platform
            }
        }
    }

}
