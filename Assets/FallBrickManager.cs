using System.Collections.Generic;
using UnityEngine;

public class FallBrickManager : MonoBehaviour
{
    public static FallBrickManager Instance { get; private set; }
    private List<FallBrick> allBricks = new List<FallBrick>();

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

    public void RegisterBrick(FallBrick brick)
    {
        if (!allBricks.Contains(brick))
        {
            allBricks.Add(brick);
        }
    }

    public void ResetAllBricks()
    {
        foreach (var brick in allBricks)
        {
            brick.ResetBrick();
        }
    }
}
