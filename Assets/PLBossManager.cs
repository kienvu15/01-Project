using System.Collections.Generic;
using UnityEngine;

public class PLBossManager : MonoBehaviour
{
    public static PLBossManager Instance { get; private set; }

    private List<PLboss> bosses = new List<PLboss>();

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

    private void Start()
    {
        // Tìm tất cả các PLboss trong scene
        bosses.AddRange(Object.FindObjectsByType<PLboss>(FindObjectsSortMode.None));
    }
    //
    // Reset tất cả boss khi Player respawn
    public void ResetAllBosses()
    {
        foreach (PLboss boss in bosses)
        {
            if (boss != null)
                boss.ResetBoss();
        }
    }
}
