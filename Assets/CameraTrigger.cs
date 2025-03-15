using UnityEngine;
using Unity.Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineCamera virtualCamera; // Camera cần thay đổi
    public Vector3 newTargetOffset = Vector3.zero; // Giá trị mới của Target Offset
    private CinemachinePositionComposer positionComposer; // Component của Cinemachine
    private Vector3 originalOffset; // Lưu giá trị Target Offset ban đầu

    private void Start()
    {
        if (virtualCamera != null)
        {
            positionComposer = virtualCamera.GetComponent<CinemachinePositionComposer>();
            if (positionComposer != null)
            {
                originalOffset = positionComposer.TargetOffset; // Lưu giá trị ban đầu
                Debug.Log("TrackedObjectOffset ban đầu: " + originalOffset);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && positionComposer != null)
        {
            Debug.Log("Player vào trigger, đổi Target Offset");
            positionComposer.TargetOffset = newTargetOffset; // Đổi offset khi nhân vật vào vùng trigger
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && positionComposer != null)
        {
            Debug.Log("Player rời trigger, trả Target Offset về ban đầu");
            positionComposer.TargetOffset = originalOffset; // Trả lại offset ban đầu khi nhân vật thoát trigger
        }
    }
}
