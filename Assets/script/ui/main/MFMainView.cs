using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MFBookInfo {
    public int id;
    public string name;
    public int playerCount;
    public float price;
    public bool isBuy;
    public Action<MFBookInfo> action;

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
    private List<MFBookInfo> bookInfoList;
    private List<GameObject> bookObjList;

    private List<MFMsgInfo> msgInfoList;
    private List<GameObject> msgObjList;

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFMainViewBind>();
        Assert.IsNotNull(uiBind);

        bookInfoList = new List<MFBookInfo>();
        bookObjList = new List<GameObject>();

        msgInfoList = new List<MFMsgInfo>();
        msgObjList = new List<GameObject>();
    }

    protected override void Start() {
        base.Start();
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
            InitBookList();
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

    private void InitBookList() {
        foreach(GameObject obj in bookObjList) {
            Destroy(obj);
        }
        bookObjList.Clear();

        List<MFBookInfo> list = GetBookList();
        using(List<MFBookInfo>.Enumerator itor = list.GetEnumerator()) {
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

    private void OnClickBook(MFBookInfo info) {
        if (info.isBuy) {
            MFUIMgr.Close<MFMainView>();
            MFBookView.Open(info.id);
        }
    }

    private List<MFBookInfo> GetBookList() {
        bookInfoList.Clear();
        for (int i = 0; i < 5; i++) {
            MFBookInfo info = new MFBookInfo {
                id = i,
                name = "办公室杀人案" + (i + 1),
                playerCount = 4,
                isBuy = true,
            };
            info.action = OnClickBook;
            bookInfoList.Add(info);
        }

        MFBookInfo info2 = new MFBookInfo {
            id = 5,
            name = "办公室超级杀人案",
            playerCount = 4,
            price = 2.5f,
            isBuy = false,
        };
        info2.action = OnClickBook;
        bookInfoList.Add(info2);

        return bookInfoList;
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
}
