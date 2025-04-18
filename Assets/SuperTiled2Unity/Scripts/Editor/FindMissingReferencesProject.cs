using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class FindMissingReferencesProject
{
    [MenuItem("Tools/Find Missing References In Project")]
    public static void FindMissingInProject()
    {
        // Quét tất cả Scene
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);

            if (!scenePath.StartsWith("Assets/")) // ⚡ Chỉ kiểm tra scene trong Assets
                continue;

            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            ReportMissingInOpenScene(scenePath);
        }


        // Quét tất cả Prefab
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in prefabGuids)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            var prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);
            ReportMissingInGameObject(prefabContents, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }

        Debug.Log("✅ Done checking project for missing references.");
    }

    // Kiểm tra Scene đang mở
    private static void ReportMissingInOpenScene(string scenePath)
    {
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int issueCount = 0;

        foreach (GameObject go in allObjects)
        {
            string path = GetFullPath(go);
            Component[] components = go.GetComponents<Component>();

            foreach (Component c in components)
            {
                if (c == null)
                {
                    Debug.LogWarning($"⚠️ Missing Component in Scene {scenePath}: {path}", go);
                    issueCount++;
                    continue;
                }

                var so = new SerializedObject(c);
                var prop = so.GetIterator();

                while (prop.NextVisible(true))
                {
                    if (prop.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (prop.objectReferenceValue == null && prop.objectReferenceInstanceIDValue != 0)
                        {
                            Debug.LogWarning($"⚠️ Missing reference in Scene {scenePath}: {path} → {c.GetType().Name}.{prop.displayName}", go);
                            issueCount++;
                        }
                    }
                }
            }
        }

        if (issueCount == 0)
            Debug.Log($"✅ No missing references found in Scene {scenePath}");
    }

    // Kiểm tra Prefab
    private static void ReportMissingInGameObject(GameObject root, string assetPath)
    {
        Component[] components = root.GetComponentsInChildren<Component>(true);

        foreach (Component c in components)
        {
            GameObject go = c ? c.gameObject : root;
            string path = go != null ? GetFullPath(go) : root.name;

            if (c == null)
            {
                Debug.LogWarning($"⚠️ Missing Component in Prefab {assetPath}: {path}");
                continue;
            }

            var so = new SerializedObject(c);
            var prop = so.GetIterator();

            while (prop.NextVisible(true))
            {
                if (prop.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (prop.objectReferenceValue == null && prop.objectReferenceInstanceIDValue != 0)
                    {
                        Debug.LogWarning($"⚠️ Missing reference in Prefab {assetPath}: {path} → {c.GetType().Name}.{prop.displayName}");
                    }
                }
            }
        }
    }

    // Lấy tên đầy đủ của GameObject trong hierarchy
    private static string GetFullPath(GameObject go)
    {
        string path = go.name;
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = go.name + "/" + path;
        }
        return path;
    }
}
