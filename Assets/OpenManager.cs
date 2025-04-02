using System.Collections.Generic;
using UnityEngine;

public class OpenManager : MonoBehaviour
{
    public static OpenManager Instance { get; private set; }

    public List<LockDoor> doors = new List<LockDoor>();
    public GameObject box; // Thêm object Box để quản lý

    public BossJump BossJump;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RegisterDoor(LockDoor door)
    {
        if (!doors.Contains(door))
        {
            doors.Add(door);
        }
    }

    private void Update()
    {
        if (BossJump.die == true) // Nếu die == true, vô hiệu hóa tất cả LockDoor và Box
        {
            DeactivateAllDoors();
            DeactivateBox();
            BossJump.die = false; // Reset trạng thái để tránh gọi nhiều lần
        }
    }

    public void DeactivateAllDoors()
    {
        foreach (LockDoor door in doors)
        {
            if (door != null)
            {
                door.Deactivate();
            }
        }
    }

    public void DeactivateBox()
    {
        if (box != null)
        {
            box.SetActive(true);
        }
    }

    // Reset lại tất cả cửa và Box
    public void ResetAll()
    {
        foreach (LockDoor door in doors)
        {
            if (door != null)
            {
                door.Actice(); // Kích hoạt lại cửa
            }
        }

        if (box != null)
        {
            box.SetActive(false); // Bật lại box
        }
    }
}
