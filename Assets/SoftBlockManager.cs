using System.Collections.Generic;
using UnityEngine;

public class SoftBlockManager : MonoBehaviour
{
    public static SoftBlockManager Instance { get; private set; }
    private List<SoftBlock> allSoftBlocks = new List<SoftBlock>();

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

    public void RegisterSoftBlock(SoftBlock softBlock)
    {
        if (!allSoftBlocks.Contains(softBlock))
        {
            allSoftBlocks.Add(softBlock);
        }
    }

    public void ResetAllSoftBlocks()
    {
        foreach (var softBlock in allSoftBlocks)
        {
            softBlock.ResetSoftBlock();
        }
    }
}
