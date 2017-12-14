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
    private List<MFPlayerInfo> _playerInfoList;
    private MFBookInfo _bookInfo;

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFPrepareRoomBind>();
        Assert.IsNotNull(uiBind);
    }

    public static void Open(int roomId, MFBookInfo bookInfo, List<MFPlayerInfo> playerInfoList) {
        MFUIMgr.Open<MFPrepareRoomView>(instance => {
            instance._roomId = roomId;
            instance._playerInfoList = playerInfoList;
            instance._bookInfo = bookInfo;
        });
    }

    protected override void OnShow() {
        base.OnShow();

        SetRoomInfo();
        InitRoomPlayerInfo();
    }

    protected override void OnEnable() {
        base.OnEnable();

        uiBind.readyBtn.onClick.AddListener(OnReadyBtnClick);
    }

    private void OnReadyBtnClick() {
        MFGameRoomView.Open();  
    }

    private void SetRoomInfo() {
        uiBind.roomName.text = string.Format("房号 {0}", _roomId);
    }

    private void InitRoomPlayerInfo() {
        for(int i = 0; i < _bookInfo.playerCount; i++) {
            GameObject bookInfoObj = Instantiate(uiBind.playerInfoTemp, uiBind.playerListPanel.transform, false);
            bookInfoObj.SetActive(true);
            if (i < _playerInfoList.Count) {
                MFGameObjectUtil.Find<Text>(bookInfoObj, "Name").text = _playerInfoList[i].name;
            } else {
                MFGameObjectUtil.Find(bookInfoObj, "Button").GetComponent<Image>().sprite = Resources.Load("texture/add2", typeof(Sprite)) as Sprite;
            }
        }
    }
}
