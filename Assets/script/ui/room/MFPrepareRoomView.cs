using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MFRoomInfo {
    public int roomId;
    public MFBookItem bookInfo;
    public List<MFPlayerInfo> playerInfoList;
}

public class MFPrepareRoomView : MFUIBase {
    private int _roomId;
    private MFPrepareRoomBind uiBind;
    private MFRoomInfo _roomInfo;

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFPrepareRoomBind>();
        Assert.IsNotNull(uiBind);
    }

    public static void Open(int roomId) {
        MFUIMgr.Open<MFPrepareRoomView>((MFPrepareRoomView inst) => {
            inst._roomId = roomId;
        });
    }

    protected override void OnShow() {
        base.OnShow();
        GetPlayerInfoList();
    }

    protected override void OnEnable() {
        base.OnEnable();

        uiBind.readyBtn.onClick.AddListener(OnReadyBtnClick);
    }

    private void OnReadyBtnClick() {
        MFGameRoomView.Open();  
    }

    private void GetPlayerInfoList() {
        MFRoomInfo info = new MFRoomInfo();
        info.roomId = 9527;
        info.playerInfoList = new List<MFPlayerInfo>();
        info.playerInfoList.Add(new MFPlayerInfo { name = "LLLL" });
        info.bookInfo = new MFBookItem {
            name = "办公室杀人案",
            playerCount = 6,
        };

        _roomInfo = info;
        OnGetRoomInfo(info);
    }

    public void OnGetRoomInfo(MFRoomInfo info) {
        MFBookItem bookInfo = info.bookInfo;
        for(int i = 0; i < bookInfo.playerCount; i++) {
            GameObject bookInfoObj = Instantiate(uiBind.playerInfoTemp, uiBind.playerListPanel.transform, false);
            bookInfoObj.SetActive(true);
            if (i < info.playerInfoList.Count) {
                MFGameObjectUtil.Find<Text>(bookInfoObj, "Name").text = info.playerInfoList[i].name;
            } else {
                MFGameObjectUtil.Find(bookInfoObj, "Button").GetComponent<Image>().sprite = Resources.Load("texture/add2", typeof(Sprite)) as Sprite;
            }
        }
    }
}
