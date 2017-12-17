using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MFBookItem {
    public int id;
    public string name;
    public int playerCount;
    public float price;
    public bool isBuy;
    public Action<MFBookItem> action;

    public void OnAction() {
        if (action != null)
            action(this);
    }
}

public class MFMsgInfo {
    public string playerName;
    public string lastMsg;
}

public class MFMainView : MFUIBase {
    private MFMainViewBind uiBind;
    private List<MFBookItem> bookItemList;
    private List<GameObject> bookObjList;

    private List<MFMsgInfo> msgInfoList;
    private List<GameObject> msgObjList;

    private MFPlayer _player;
    private List<MFBook> _bookList;

    public static void Open(MFPlayer player, List<MFBook> bookList) {
        MFUIMgr.Open<MFMainView>( instance => {
            instance._player = player;
            instance._bookList = bookList;
        });
    }

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFMainViewBind>();
        Assert.IsNotNull(uiBind);

        bookItemList = new List<MFBookItem>();
        bookObjList = new List<GameObject>();

        msgInfoList = new List<MFMsgInfo>();
        msgObjList = new List<GameObject>();

        //if (MFApplicationUtil.IsOpenDebug()) {
            uiBind.testEntrance.gameObject.SetActive(true);
            uiBind.testEntrance.onClick.AddListener(() => {
                MFUIMgr.Close<MFMainView>();
                MFUIMgr.Open<MFAgoraTestView>();
            });
        //}
    }

    protected override void Start() {
        base.Start();
        MFAgoraMgr.JoinChannel("9527");
    }

    protected override void OnShow() {
        base.OnShow();

        SetPlayerInfo();
        InitBookItemList(_bookList);
    }

    protected override void OnEnable() {
        base.OnEnable();

        AddToggleListener();
        uiBind.bookListToggle.Select();
        uiBind.bookListToggle.isOn = true;
    }

    protected override void OnDisable() {
        base.OnDisable();

        RemoveToggleListener();
    }

    private void AddToggleListener() {
        uiBind.bookListToggle.onValueChanged.AddListener(OnBookToggleChange);
        uiBind.msgListToggle.onValueChanged.AddListener(OnMsgToggleChange);
    }

    private void RemoveToggleListener() {
        uiBind.bookListToggle.onValueChanged.RemoveListener(OnBookToggleChange);
        uiBind.msgListToggle.onValueChanged.RemoveListener(OnMsgToggleChange);
    }

    private void OnBookToggleChange(bool isOn) {
        if (isOn) {
            InitBookItemObjectList();
            uiBind.bookPanel.SetActive(true);
        } else {
            uiBind.bookPanel.SetActive(false);
        }
    }

    private void OnMsgToggleChange(bool isOn) {
        if (isOn) {
            InitMsgInfoList();
            uiBind.msgPanel.SetActive(true);
        } else {
            uiBind.msgPanel.SetActive(false);
        }
    }

    // 创建本子列表
    private void InitBookItemList(List<MFBook> bookList) {
        bookItemList.Clear();
        foreach (MFBook book in bookList) {
            MFBookItem bookItem = new MFBookItem {
                id = book.GetId(),
                name = book.GetName(),
                playerCount = book.GetPlayerCount(),
                price = book.GetPrice(),
                isBuy = book.IsBuy(),
                action = OnBookClick,
            };
            bookItemList.Add(bookItem);
        }

        InitBookItemObjectList();
    }

    // 创建本子GameObject列表
    private void InitBookItemObjectList() {
        foreach(GameObject obj in bookObjList) {
            Destroy(obj);
        }
        bookObjList.Clear();

        using (List<MFBookItem>.Enumerator itor = bookItemList.GetEnumerator()) {
            while (itor.MoveNext()) {
                GameObject bookInfoObj = Instantiate(uiBind.bookTemplate, uiBind.bookListContentView.transform, false);
                bookInfoObj.SetActive(true);
                Text bookName = MFGameObjectUtil.Find<Text>(bookInfoObj, "Name");
                bookName.text = itor.Current.name;
                Text bookPlayerCount = MFGameObjectUtil.Find<Text>(bookInfoObj, "PlayerCount");
                bookPlayerCount.text = itor.Current.playerCount.ToString();
                GameObject openBookBtn = MFGameObjectUtil.Find(bookInfoObj, "Open");
                GameObject buyBookBtn = MFGameObjectUtil.Find(bookInfoObj, "Buy");
                if (itor.Current.isBuy) {
                    buyBookBtn.SetActive(false);
                    openBookBtn.SetActive(true);
                    openBookBtn.GetComponent<Button>().onClick.AddListener(itor.Current.OnAction);
                } else {
                    openBookBtn.SetActive(false);
                    buyBookBtn.SetActive(true);
                    MFGameObjectUtil.Find<Text>(buyBookBtn, "Text").text = itor.Current.price.ToString();
                    buyBookBtn.GetComponent<Button>().onClick.AddListener(itor.Current.OnAction);
                }

                bookObjList.Add(bookInfoObj);
            }
        }
    }

    // 点击本子下面的按钮
    private void OnBookClick(MFBookItem info) {
        if (info.isBuy) {
            MFUIMgr.Close<MFMainView>();
            MFBookView.Open(info.id);
        }
    }

    private void InitMsgInfoList() {
        foreach (GameObject obj in msgObjList) {
            Destroy(obj);
        }
        msgObjList.Clear();

        List<MFMsgInfo> list = GetMsgInfoList();
        using (List<MFMsgInfo>.Enumerator itor = list.GetEnumerator()) {
            while (itor.MoveNext()) {
                GameObject msgInfoObj = Instantiate(uiBind.msgTemplate, uiBind.msgListContentView.transform, false);
                msgInfoObj.SetActive(true);
                Text bookName = MFGameObjectUtil.Find<Text>(msgInfoObj, "PlayerName");
                bookName.text = itor.Current.playerName;
                Text bookPlayerCount = MFGameObjectUtil.Find<Text>(msgInfoObj, "LastMsg");
                bookPlayerCount.text = itor.Current.lastMsg;

                msgObjList.Add(msgInfoObj);
            }
        }
    }

    private List<MFMsgInfo> GetMsgInfoList() {
        msgInfoList.Clear();

        for(int i = 0; i < 10; i++) {
            MFMsgInfo info = new MFMsgInfo {
                playerName = "夜神月",
                lastMsg = "?",
            };

            msgInfoList.Add(info);
        }

        return msgInfoList;
    }

    // 设置玩家信息
    private void SetPlayerInfo() {
        uiBind.playerName.text = _player.GetName();
        uiBind.playerLevel.text = _player.GetLevel().ToString();
    }
}
