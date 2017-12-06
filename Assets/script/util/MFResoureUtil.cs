using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MFResoureUtil {
    private static readonly string prefabPrefixPath = "prefab";

    public static GameObject LoadPrefabFromPath(string path) {
        return Resources.Load(string.Format("{0}/{1}", prefabPrefixPath, path), typeof(GameObject)) as GameObject;
        //return AssetDatabase.LoadAssetAtPath(string.Format("{0}/{1}", prefabPrefixPath, path),
        //    typeof(GameObject)) as GameObject;
    }
}
