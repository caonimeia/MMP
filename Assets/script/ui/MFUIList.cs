using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrefabPath {
    private const string prePath = "ui/";
    public const string loginView = prePath + "login/LoginView";
    public const string mainView = prePath + "main/MainView";
    public const string bookView = prePath + "main/BookView";
    public const string prepareRoomView = prePath + "room/PrepareRoom";
}

public static class MFUIList {
    public static void Bind() {
        MFUIMgr.BindPrefab<MFLoginView>(UIPrefabPath.loginView, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFMainView>(UIPrefabPath.mainView, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFBookView>(UIPrefabPath.bookView, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFPrepareRoomView>(UIPrefabPath.prepareRoomView, UILayer.main, UIInstanceType.single);
    }
}
