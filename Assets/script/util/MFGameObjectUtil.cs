using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MFGameObjectUtil {

    /// <summary>
    /// 获取GameObject或Component的transform组件
    /// </summary>
    public static Transform GetTransform(Object target) {
        Transform transform = target as Transform;
        if (transform)
            return transform;

        Component component = target as Component;
        if (component)
            return component.transform;

        GameObject gameObject = target as GameObject;
        if (gameObject)
            return gameObject.transform;

        return null;
    }

    /// <summary>
    /// 通过路径查找GameObject或Component中子节点的Component
    /// </summary>
    public static T Find<T>(Object target, string path) where T : Component {
        T targetT = null;
        Transform transform = GetTransform(target);
        if (transform)
            if (transform = transform.Find(path)) {
                targetT = transform.GetComponent<T>();
                if (targetT == null) {
                    targetT = transform.gameObject.AddComponent<T>();
                }
            }

        if (!targetT)
            MFLog.LogError(string.Format("{0}[path={1}] is not found!", typeof(T), path));

        return targetT;
    }

    /// <summary>
    /// 通过路径查找GameObject或Component中子节点(GameObject)
    /// </summary>
    public static GameObject Find(Object target, string path) {
        GameObject targetGo = null;
        Transform transform = GetTransform(target);
        if (transform)
            if (transform = transform.Find(path))
                targetGo = transform.gameObject;

        if (!target)
            MFLog.LogError(string.Format("GameObject[path={0}] is not found!", path));

        return targetGo;
    }

    /// <summary>
    /// 获取GameObject根节点
    /// </summary>
    public static GameObject GetRootObject(GameObject childObject) {
        if (childObject.transform.parent == null) 
            return childObject;
        else 
            return GetRootObject(childObject);
    }
}
