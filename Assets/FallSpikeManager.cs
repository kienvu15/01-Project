using System.Collections.Generic;
using UnityEngine;

public class FallSpikeManager : MonoBehaviour
{
    public static FallSpikeManager Instance { get; private set; }

    private List<FallSpike> allFallSpikes = new List<FallSpike>();

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

        // Tự động lấy tất cả FallSpike trong scene (kể cả khi là con)
        allFallSpikes.AddRange(Object.FindObjectsByType<FallSpike>(FindObjectsSortMode.None));


    }

    public void RegisterFallSpike(FallSpike spike)
    {
        if (!allFallSpikes.Contains(spike))
        {
            allFallSpikes.Add(spike);
        }
    }

    public void ResetAllSpikes()
    {
        foreach (var spike in allFallSpikes)
        {
            if (spike != null)
                spike.ResetBrick();
        }
    }
}
