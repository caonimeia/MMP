using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MFRoomInfo {
    public int roomId;
    public int roomMasterId;
    public MFBookInfo bookInfo;
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

    public static void Open(int roomId, int roomMasterId, MFBookInfo bookInfo, List<MFPlayerInfo> playerInfoList) {
        MFUIMgr.Open<MFPrepareRoomView>(instance => {
            instance._roomInfo = new MFRoomInfo {
                roomId = roomId,
                roomMasterId = roomMasterId,
                bookInfo = bookInfo,
                playerInfoList = playerInfoList,
            };
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
          
    }

    private void SetRoomInfo() {
        uiBind.roomName.text = string.Format("房号 {0}", _roomInfo.roomId);
        if (_roomInfo.roomMasterId == GameAgent.curPlayer.GetId()) {
            uiBind.startBtn.gameObject.SetActive(true);
            uiBind.readyBtn.gameObject.SetActive(false);
        } else {
            uiBind.readyBtn.gameObject.SetActive(true);
            uiBind.startBtn.gameObject.SetActive(false);
        }
    }

    private void InitRoomPlayerInfo() {
        for(int i = 0; i < _roomInfo.bookInfo.playerCount; i++) {
            GameObject bookInfoObj = Instantiate(uiBind.playerInfoTemp, uiBind.playerListPanel.transform, false);
            bookInfoObj.SetActive(true);
            if (i < _roomInfo.playerInfoList.Count) {
                MFGameObjectUtil.Find<Text>(bookInfoObj, "Name").text = _roomInfo.playerInfoList[i].name;
            } else {
                MFGameObjectUtil.Find(bookInfoObj, "Button").GetComponent<Image>().sprite = Resources.Load("texture/add2", typeof(Sprite)) as Sprite;
            }
        }
    }

    #region 服务器响应
    public void OnReadyToStartRespond(MFRespondHeader header, MFReadyToStartRespond data) {
        if(header.result == 0) {
            MFLog.LogInfo("玩家准备完成");
        }
    }
    #endregion
}
