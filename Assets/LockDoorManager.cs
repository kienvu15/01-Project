using System.Collections.Generic;
using UnityEngine;

public class LockDoorManager : MonoBehaviour
{
    public static LockDoorManager Instance { get; private set; }
    private List<LockDoor> allDoors = new List<LockDoor>();

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
        if (!allDoors.Contains(door))
        {
            allDoors.Add(door);
        }
    }

    public void ResetAllDoors()
    {
        foreach (var door in allDoors)
        {
            door.Actice(); // Gọi hàm bật lại cửa
        }
    }
}
