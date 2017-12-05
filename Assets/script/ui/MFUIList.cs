using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrefabPath {
    private const string prePath = "ui/";
    public const string loginView = prePath + "login/LoginView";
    public const string mainView = prePath + "main/MainView";
}

public static class MFUIList {
    public static void Bind() {
        MFUIMgr.BindPrefab<MFLoginView>(UIPrefabPath.loginView, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFMainView>(UIPrefabPath.mainView, UILayer.main, UIInstanceType.single);
    }
}
