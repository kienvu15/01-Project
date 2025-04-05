using System.Collections.Generic;
using UnityEngine;

public class RunEnemyManager : MonoBehaviour
{
    public static RunEnemyManager Instance { get; private set; }

    private List<Runenemy> allEnemies = new List<Runenemy>();

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
        // Tìm tất cả các enemy Runenemy trong scene
        allEnemies.AddRange(FindObjectsByType<Runenemy>(FindObjectsSortMode.None));
    }

    public void ResetAllEnemies()
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy != null)
                enemy.ResetEnemy();
        }
    }

    public void RegisterEnemy(Runenemy enemy)
    {
        if (!allEnemies.Contains(enemy))
            allEnemies.Add(enemy);
    }

}
