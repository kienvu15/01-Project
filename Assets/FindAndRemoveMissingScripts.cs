using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ReportMissingScripts
{
    [MenuItem("Tools/Report Missing Scripts In Project")]
    static void ReportAll()
    {
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        List<string> objectsWithMissingScripts = new List<string>();

        for (int i = 0; i < allAssetPaths.Length; i++)
        {
            string path = allAssetPaths[i];
            if (!path.StartsWith("Assets")) continue;

            Object asset = AssetDatabase.LoadMainAssetAtPath(path);

            if (asset is GameObject go)
            {
                Component[] components = go.GetComponentsInChildren<Component>(true);

                foreach (Component comp in components)
                {
                    if (comp == null)
                    {
                        objectsWithMissingScripts.Add(path);
                        break; // chỉ cần báo 1 lần cho mỗi GameObject
                    }
                }
            }
        }

        if (objectsWithMissingScripts.Count > 0)
        {
            Debug.LogWarning($"🚨 Found {objectsWithMissingScripts.Count} GameObjects with Missing Scripts!");

            foreach (string objPath in objectsWithMissingScripts)
            {
                Debug.LogWarning($"⚠️ Missing Script detected in: {objPath}");
            }
        }
        else
        {
            Debug.Log("✅ No Missing Scripts found in project!");
        }
    }
}
