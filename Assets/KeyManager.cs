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
}
