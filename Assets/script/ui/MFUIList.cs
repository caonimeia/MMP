using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrefabPath {
    private const string PRE_PATH = "ui/";
    public const string LOGIN_VIEW = PRE_PATH + "login/LoginView";
    public const string MAIN_VIEW = PRE_PATH + "main/MainView";
    public const string BOOK_VIEW = PRE_PATH + "main/BookView";
    public const string PREPARE_ROOM_VIEW = PRE_PATH + "room/PrepareRoomView";
    public const string GAME_ROOM_VIEW = PRE_PATH + "room/GameRoomView";
}

public static class MFUIList {
    public static void Bind() {
        MFUIMgr.BindPrefab<MFLoginView>(UIPrefabPath.LOGIN_VIEW, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFMainView>(UIPrefabPath.MAIN_VIEW, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFBookView>(UIPrefabPath.BOOK_VIEW, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFPrepareRoomView>(UIPrefabPath.PREPARE_ROOM_VIEW, UILayer.main, UIInstanceType.single);
        MFUIMgr.BindPrefab<MFGameRoomView>(UIPrefabPath.GAME_ROOM_VIEW, UILayer.main, UIInstanceType.single);
    }
}
