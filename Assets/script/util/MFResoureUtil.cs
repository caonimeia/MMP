using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MFResoureUtil {
    private static readonly string prefabPrefixPath = "Assets/prefab";

    public static GameObject LoadPrefabFromPath(string path) {
        return AssetDatabase.LoadAssetAtPath(string.Format("{0}/{1}", prefabPrefixPath, path),
            typeof(GameObject)) as GameObject;
    }
}
