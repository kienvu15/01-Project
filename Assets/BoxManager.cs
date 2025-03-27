using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance { get; private set; }
    private List<Box> allBoxes = new List<Box>();

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

    public void RegisterBox(Box box)
    {
        if (!allBoxes.Contains(box))
        {
            allBoxes.Add(box);
        }
    }

    public void ResetAllBoxes()
    {
        foreach (var box in allBoxes)
        {
            box.ResetBox();
        }
    }
}
