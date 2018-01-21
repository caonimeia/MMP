using System;
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

public class MFCharacterItem {
    public MFCharacterInfo characterInfo;
    public Action<MFCharacterItem> action;

    public void OnAction() {
        if (action != null)
            action(this);
    }
}

public class MFPrepareRoomView : MFUIBase {
    //private int _roomId;
    private MFPrepareRoomBind uiBind;
    private MFRoomInfo _roomInfo;
    //private List<MFPlayerInfo> _playerInfoList;
    //private MFBookInfo _bookInfo;
    private List<GameObject> _playerInfoObjList;
    private List<GameObject> _characterInfoObjList; //剧本角色列表
    private List<MFCharacterItem> _characterInfoList;

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFPrepareRoomBind>();
        Assert.IsNotNull(uiBind);

        _playerInfoObjList = new List<GameObject>();
        _characterInfoObjList = new List<GameObject>();
        _characterInfoList = new List<MFCharacterItem>();
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
        uiBind.characterListPanel.SetActive(false);


        // 加入频道 静音自己
        MFAgoraMgr.JoinChannel(_roomInfo.roomId.ToString());
        MFAgoraMgr.mRtcEngine.MuteLocalAudioStream(true);
    }

    protected override void OnEnable() {
        base.OnEnable();

        

        uiBind.readyBtn.onClick.AddListener(OnReadyBtnClick);
        uiBind.speakBtn.onPointerDown.AddListener(OnSpeakBtnDown);
        uiBind.speakBtn.onPointerUp.AddListener(OnSpeakBtnUp);
        uiBind.selectCharacterBtn.onClick.AddListener(OnShowCharacterBtnClick);
        uiBind.startBtn.onClick.AddListener(OnStartGameBtnClick);
        uiBind.sendScriptBtn.onClick.AddListener(OnSendScriptBtnClick);
    }


    protected override void OnDisable() {
        base.OnDisable();


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
            uiBind.startBtn.gameObject.SetActive(false);
            uiBind.startBtn.enabled = false;

            uiBind.readyBtn.gameObject.SetActive(false);
            uiBind.readyBtn.enabled = false;

            uiBind.sendScriptBtn.gameObject.SetActive(true);
            uiBind.sendScriptBtn.enabled = false;
        } else {
            uiBind.startBtn.gameObject.SetActive(false);
            uiBind.startBtn.enabled = false;

            uiBind.sendScriptBtn.gameObject.SetActive(false);
            uiBind.sendScriptBtn.enabled = false;

            uiBind.readyBtn.gameObject.SetActive(true);
            uiBind.readyBtn.enabled = false;
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
            if (item.name == GameAgent.curPlayer.GetName())
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

    private void OnShowCharacterBtnClick() {
        MFServerAgentBase.Send(MFProtocolId.getCharacterListRequest, _roomInfo.roomId);
    }


    private void OnSelectCharacterBtnClick(MFCharacterItem item) {
        uiBind.characterListPanel.SetActive(false);
        MFServerAgentBase.Send(MFProtocolId.selectCharacterRequest, item.characterInfo.roleId, _roomInfo.roomId);
    }

    private void InitCharacterInfoList(List<MFCharacterInfo> roleList) {
        _characterInfoList.Clear();
        foreach (var item in roleList) {
            MFCharacterItem characterItem = new MFCharacterItem();
            characterItem.characterInfo.roleId = item.roleId;
            characterItem.characterInfo.codeId = item.codeId;
            characterItem.characterInfo.isSelect = item.isSelect;
            characterItem.characterInfo.desc = item.desc;

            characterItem.action = OnSelectCharacterBtnClick;
            _characterInfoList.Add(characterItem);
        }
    }

    private void RefreshCharacterInfoList(List<MFCharacterInfo> roleList) {
        InitCharacterInfoList(roleList);
        MFLog.LogError(isRoomMaster());
        MFLog.LogError(AllPlayerSelect(roleList));

        if (isRoomMaster() && AllPlayerSelect(roleList)) {
            uiBind.sendScriptBtn.gameObject.SetActive(true);
            uiBind.sendScriptBtn.enabled = true;
        }
    }

    private bool AllPlayerSelect(List<MFCharacterInfo> roleList) {
        foreach (var item in roleList) {
            if(!item.isSelect)
                return false;
        }

        return true;
    }

    private void InitCharacterInfoObjList() {
        foreach (GameObject obj in _characterInfoObjList) {
            Destroy(obj);
        }
        _characterInfoObjList.Clear();

        using (List<MFCharacterItem>.Enumerator itor = _characterInfoList.GetEnumerator()) {
            while (itor.MoveNext()) {
                GameObject bookInfoObj = Instantiate(uiBind.characterInfoTmp, uiBind.characterListPanel.transform, false);
                bookInfoObj.SetActive(true);
                Button btn = MFGameObjectUtil.Find<Button>(bookInfoObj, "Button");
                Text text = MFGameObjectUtil.Find<Text>(btn, "Text");
                text.text = itor.Current.characterInfo.desc;
                btn.onClick.AddListener(itor.Current.OnAction);
                btn.enabled = !itor.Current.characterInfo.isSelect;

                _characterInfoObjList.Add(bookInfoObj);
            }
        }
    }

    private void OnStartGameBtnClick() {
        //MFServerAgentBase.Send(MFProtocolId.)
    }


    private void OnSendScriptBtnClick() {
        MFServerAgentBase.Send(MFProtocolId.sendCharacterScriptRequest, _roomInfo.roomId);
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

    public void OnGetCharacterListRespond(MFRespondHeader header, MFGetCharacterListRespond data) {
        InitCharacterInfoList(data.roleList);
        uiBind.characterListPanel.SetActive(true);
        InitCharacterInfoObjList();
    }

    public void OnSelectCharacterRespond(MFRespondHeader header, MFSelectCharacterRespond data) {
        if(header.result == 0) {
            if (header.broadcast) {
                // 更新角色列表状态
                RefreshCharacterInfoList(data.roleList);
            } else {
                if (data.result) {
                    RefreshCharacterInfoList(data.roleList);
                } else {
                    MFLog.LogError("选择失败");
                }
            }
        }
    }

    public void OnSendCharacterScriptRespond(MFRespondHeader header, MFSendCharacterScriptRespond data) {
        if(header.result == 0) {
            MFLog.LogInfo("发放成功");
            MFLog.LogInfo(data.script);
        }
    }
    #endregion
}
