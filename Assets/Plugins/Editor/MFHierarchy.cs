using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public class MFHierarchy {
    private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;
    static MFHierarchy() {
        hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(DrawActiveToggle);
        EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
            EditorApplication.hierarchyWindowItemOnGUI,hiearchyItemCallback);
    }

    private static void DrawActiveToggle(int instanceID, Rect selectionRect) {
        bool isOk = true;
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        EditorGUI.BeginChangeCheck();
        if (obj) 
            isOk = EditorGUI.Toggle(new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f), obj.activeSelf);

        if (EditorGUI.EndChangeCheck())
            obj.SetActive(isOk);
    }
}
