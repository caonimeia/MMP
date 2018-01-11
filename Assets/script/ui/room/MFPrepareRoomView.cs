using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MFRoomInfo {
    public int roomId;
    //public int roomMasterId;
    //public MFBookInfo bookInfo;
    public List<MFPrepareRoomPlayerInfo> playerInfoList;
    public int roomMaxPlayerCount; //本子能容纳的最大玩家数量
}

public class MFPrepareRoomView : MFUIBase {
    //private int _roomId;
    private MFPrepareRoomBind uiBind;
    private MFRoomInfo _roomInfo;
    //private List<MFPlayerInfo> _playerInfoList;
    //private MFBookInfo _bookInfo;
    private List<GameObject> _playerInfoObjList;

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFPrepareRoomBind>();
        Assert.IsNotNull(uiBind);

        _playerInfoObjList = new List<GameObject>();
    }

    public static void Open(int roomId, int roomMaxPlayerCount, List<MFPrepareRoomPlayerInfo> playerInfoList) {
        MFUIMgr.Open<MFPrepareRoomView>(instance => {
            instance._roomInfo = new MFRoomInfo {
                roomId = roomId,
                //roomMasterId = roomMasterId,
                //bookInfo = bookInfo,
                playerInfoList = playerInfoList,
                roomMaxPlayerCount = roomMaxPlayerCount,
            };
        });
    }

    protected override void OnShow() {
        base.OnShow();

        SetRoomInfo();
        InitRoomPlayerInfo();

        // 加入频道 静音自己
        MFAgoraMgr.JoinChannel(_roomInfo.roomId.ToString());
        MFAgoraMgr.mRtcEngine.MuteLocalAudioStream(true);
    }

    protected override void OnEnable() {
        base.OnEnable();

        uiBind.readyBtn.onClick.AddListener(OnReadyBtnClick);
        uiBind.speakBtn.onPointerDown.AddListener(OnSpeakBtnDown);
        uiBind.speakBtn.onPointerUp.AddListener(OnSpeakBtnUp);
    }

    private void OnReadyBtnClick() {
          
    }

    private void OnSpeakBtnDown() {
        MFAgoraMgr.PressToSpeakBegin();
    }

    private void OnSpeakBtnUp() {
        MFAgoraMgr.PressToSpeakEnd();
    }

    private void SetRoomInfo() {
        uiBind.roomName.text = string.Format("房号 {0}", _roomInfo.roomId);
        if (isRoomMaster()) {
            uiBind.startBtn.gameObject.SetActive(true);
            uiBind.readyBtn.gameObject.SetActive(false);
        } else {
            uiBind.readyBtn.gameObject.SetActive(true);
            uiBind.startBtn.gameObject.SetActive(false);
        }
    }

    private void InitRoomPlayerInfo() {
        ClearPlayerInfoObjList();

        for (int i = 0; i < _roomInfo.roomMaxPlayerCount; i++) {
            GameObject playerInfoObj = Instantiate(uiBind.playerInfoTemp, uiBind.playerListPanel.transform, false);
            playerInfoObj.SetActive(true);
            if (i < _roomInfo.playerInfoList.Count) {
                MFGameObjectUtil.Find<Text>(playerInfoObj, "Name").text = _roomInfo.playerInfoList[i].name;
            } else {
                MFGameObjectUtil.Find(playerInfoObj, "Button").GetComponent<Image>().sprite = Resources.Load("texture/add2", typeof(Sprite)) as Sprite;
            }

            _playerInfoObjList.Add(playerInfoObj);
        }
    }

    private void ClearPlayerInfoObjList() {
        foreach(GameObject obj in _playerInfoObjList) {
            Destroy(obj);
        }

        _playerInfoObjList.Clear();
    }

    private bool isRoomMaster() {
        foreach(var item in _roomInfo.playerInfoList) {
            if(item.name == GameAgent.curPlayer.GetName())
                return item.isRoomOwner;
        }

        MFLog.LogError("没有找到房主");
        return false;
    }

    public void RefreshPlayerList(List<MFPrepareRoomPlayerInfo> playerInfoList) {
        _roomInfo.playerInfoList = playerInfoList;
        SetRoomInfo();
        InitRoomPlayerInfo();
    }



    #region 服务器响应
    public void OnReadyToStartRespond(MFRespondHeader header, MFReadyToStartRespond data) {
        if(header.result == 0) {
            MFLog.LogInfo("玩家准备完成");
        }
    }

    // 加入房间 静态的原因是还没有创建实例
    public static void OnJoinRoomRespond(MFRespondHeader header, MFJoinRoomRespond data) {
        if (header.result == 0) {
            Open(data.roomNumber, data.playerCount, data.userList);
        }
    }

    #endregion
}
