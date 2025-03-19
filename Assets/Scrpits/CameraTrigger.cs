using UnityEngine;
using Unity.Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineCamera virtualCamera; // Camera cần thay đổi
    public Vector3 newTargetOffset = Vector3.zero; // Giá trị mới của Target Offset
    public float transitionSpeed = 2f; // Tốc độ chuyển đổi Offset

    private CinemachinePositionComposer positionComposer; // Component của Cinemachine
    private Vector3 originalOffset; // Lưu giá trị Target Offset ban đầu
    private Vector3 targetOffset; // Giá trị Offset cần chuyển đến
    private bool isChangingOffset = false; // Kiểm tra xem có đang thay đổi Offset không

    private void Start()
    {
        if (virtualCamera != null)
        {
            positionComposer = virtualCamera.GetComponent<CinemachinePositionComposer>();
            if (positionComposer != null)
            {
                originalOffset = positionComposer.TargetOffset; // Lưu giá trị ban đầu
                targetOffset = originalOffset;
            }
        }
    }

    private void Update()
    {
        if (isChangingOffset && positionComposer != null)
        {
            // Dùng Lerp để chuyển đổi Offset mượt hơn
            positionComposer.TargetOffset = Vector3.Lerp(
                positionComposer.TargetOffset,
                targetOffset,
                Time.deltaTime * transitionSpeed
            );

            // Kiểm tra nếu giá trị gần đạt mục tiêu thì dừng
            if (Vector3.Distance(positionComposer.TargetOffset, targetOffset) < 0.01f)
            {
                positionComposer.TargetOffset = targetOffset;
                isChangingOffset = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && positionComposer != null)
        {
            targetOffset = newTargetOffset; // Đặt mục tiêu Offset mới
            isChangingOffset = true; // Bắt đầu thay đổi
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && positionComposer != null)
        {
            targetOffset = originalOffset; // Quay lại Offset ban đầu
            isChangingOffset = true; // Bắt đầu thay đổi
        }
    }
}
