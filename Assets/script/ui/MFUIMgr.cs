using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System;


public enum UILayer {
    main,
    fight,
}

public enum UIDisplayType {
    _2D,
    _3D,
}

public enum UIInstanceType {
    single,
    multi,
}

public static class MFUIMgr {
    private class UIBindInfo {
        public GameObject prefab;
        public UILayer layer;
        public UIInstanceType instType;
    }

    private struct UIAliveInfo {
        public int lastOpenTime;

    }

    //需要update 检测ui存活时间

    private static Dictionary<Type, UIBindInfo> _uiInfobDic;
    private static Dictionary<Type, GameObject> _aliveUI;
    private static GameObject _mainUILayer;
    private const int _uiLiveTime = 180;

    public static Camera camera2D;

    static MFUIMgr() {
        _uiInfobDic = new Dictionary<Type, UIBindInfo>();
        _aliveUI = new Dictionary<Type, GameObject>();
    }

    public static void Init() {
        camera2D = GameObject.Find("UIRoot/2DCamera").GetComponent<Camera>();
        Assert.IsNotNull(camera2D);
        GameObject.DontDestroyOnLoad(camera2D.transform.parent.gameObject);

        _mainUILayer = GameObject.Find("UIRoot/2DLayer/Main");
        Assert.IsNotNull(_mainUILayer);

        MFUIList.Bind();
    }

    /// <summary>
    /// 打开UI UI还存活的直接显示，否则创建新的UI。
    /// 最后会调用OnShow
    /// </summary>
    public static void Open<T>() where T : MFUIBase {
        Type uiScript = typeof(T);
        if (!IsBind<T>()) {
            MFLog.LogError("UI脚本没有绑定Prefab");
            return;
        }

        if (IsAlive<T>()) {
            Show<T>();
            return;
        }

        CreateNewUI<T>();
    }

    /// <summary>
    /// 关闭UI 单例的隐藏 非单例的销毁
    /// </summary>
    public static void Close<T>() where T : MFUIBase {
        if (!IsBind<T>())
            return;

        Type uiScript = typeof(T);
        UIBindInfo uiInfo = _uiInfobDic[uiScript];
        GameObject uiObj = _aliveUI[uiScript];
        uiObj.GetComponent<T>().Invoke("OnClose", 0);
        if (uiInfo.instType == UIInstanceType.single) {
            uiObj.SetActive(false);
        } else {
            _aliveUI.Remove(uiScript);
            GameObject.Destroy(uiObj);
        }
    }

    private static void Show<T>() where T : MFUIBase {
        GameObject uiObj = _aliveUI[typeof(T)];
        uiObj.SetActive(true);
        uiObj.GetComponent<T>().Invoke("OnShow", 0);
    }

    private static bool IsBind<T>() where T : MFUIBase {
        if (!_uiInfobDic.ContainsKey(typeof(T)))
            return false;

        return true;
    }

    private static bool IsAlive<T>() where T : MFUIBase {
        if (!_aliveUI.ContainsKey(typeof(T)))
            return false;

        return true;
    }

    private static void CreateNewUI<T>() where T : MFUIBase {
        Type uiScript = typeof(T);
        UIBindInfo uiInfo = _uiInfobDic[uiScript];
        GameObject uiObj = GameObject.Instantiate(uiInfo.prefab);
        Transform parent = _mainUILayer.transform; // default
        switch (uiInfo.layer) {
            case UILayer.main:
                parent = _mainUILayer.transform;
                break;
            case UILayer.fight:
                break;
        }

        uiObj.transform.SetParent(parent, false);
        _aliveUI.Add(uiScript, uiObj);
        Show<T>();
    }

    /// <summary>
    /// 将UI脚本组件与Prefab绑定
    /// </summary>
    public static void BindPrefab<T>(string prefabPath, UILayer layer, UIInstanceType instType) where T : MFUIBase {
        Type uiScript = typeof(T);
        GameObject uiPrefab = MFResoureUtil.LoadPrefabFromPath(prefabPath);
        Assert.IsNotNull(uiPrefab);
        UIBindInfo info = new UIBindInfo {
            prefab = uiPrefab,
            layer = layer,
            instType = instType,
        };

        if (_uiInfobDic.ContainsKey(uiScript)) {
            MFLog.LogError("UI脚本重复绑定!!!");
        }

        _uiInfobDic.Add(uiScript, info);
    }
}
