using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance { get; private set; }
    private List<Key> allKeys = new List<Key>();

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

    public void RegisterKey(Key key)
    {
        if (!allKeys.Contains(key))
        {
            allKeys.Add(key);
        }
    }

    public void ResetAllKeys()
    {
        foreach (var key in allKeys)
        {
            key.ResetKey(); // Reset trạng thái Key
        }
    }


    public void StartOpeningDoors(GameObject[] doors)
    {
        StartCoroutine(OpenDoorsInOrder(doors));
    }

    private IEnumerator OpenDoorsInOrder(GameObject[] doors)
    {
        foreach (GameObject door in doors)
        {
            if (door == null) continue;

            LockDoor lockDoorScript = door.GetComponent<LockDoor>();
            if (lockDoorScript != null)
            {
                lockDoorScript.PlayAnimation();
            }

            yield return new WaitForSeconds(1f); // Chờ trước khi mở cửa tiếp theo
        }
    }


}
