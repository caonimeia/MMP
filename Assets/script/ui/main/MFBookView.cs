using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MFBookView : MFUIBase {
    public static void Open(int bookId) {
        MFUIMgr.Open<MFBookView>((MFBookView instance) => {
            instance._currBookId = bookId;
        });
    }

    private MFBookViewBind uiBind;
    private int _currBookId;
    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFBookViewBind>();
        Assert.IsNotNull(uiBind);

        MFServerAgent.RegisterRpcCallBack<MFGetBookDetailRespond>(MFProtocolId.getBookDetailRespond, OnGetBookDetailRespond);
    }

    protected override void Start() {
        base.Start();
    }

    protected override void OnEnable() {
        base.OnEnable();

        AddToggleListener();
        AddBtnListener();

        uiBind.backStoryToggle.Select();
        uiBind.backStoryToggle.isOn = true;
    }

    protected override void OnShow() {
        base.OnShow();

        MFServerAgent.DoGetBookDetailRequest(_currBookId);
    }

    protected override void OnDisable() {
        base.OnDisable();

        RemoveToggleListener();
        RemoveBtnListener();
    }

    protected override void OnDestroy() {
        base.OnDestroy();
    }

    private void OnClickBackBtn() {
        MFUIMgr.Close<MFBookView>();
        MFUIMgr.Open<MFMainView>();
    }

    private void AddBtnListener() {
        uiBind.backBtn.onClick.AddListener(OnClickBackBtn);
        uiBind.reserveRoomBtn.onClick.AddListener(OnReserverRoomBtnClick);
        uiBind.openRoomBtn.onClick.AddListener(OnOpenRoomBtnClick);
    }

    private void RemoveBtnListener() {
        uiBind.backBtn.onClick.RemoveListener(OnClickBackBtn);
        uiBind.reserveRoomBtn.onClick.RemoveListener(OnReserverRoomBtnClick);
        uiBind.openRoomBtn.onClick.RemoveListener(OnOpenRoomBtnClick);
    }

    private void OnReserverRoomBtnClick() {
        MFLog.LogInfo("OnReserverRoomBtnClick");
    }

    private void OnOpenRoomBtnClick() {
        MFPrepareRoomView.Open(9527);
    }

    private void AddToggleListener() {
        uiBind.backStoryToggle.onValueChanged.AddListener(OnBackStoryToggleChange);
        uiBind.gameRuleToggle.onValueChanged.AddListener(OnGameRuleToggleChange);
        uiBind.characterInfoToggle.onValueChanged.AddListener(OnCharacterInfoToggleChange);
    }

    private void RemoveToggleListener() {
        uiBind.backStoryToggle.onValueChanged.RemoveListener(OnBackStoryToggleChange);
        uiBind.gameRuleToggle.onValueChanged.RemoveListener(OnGameRuleToggleChange);
        uiBind.characterInfoToggle.onValueChanged.RemoveListener(OnCharacterInfoToggleChange);
    }

    private void OnBackStoryToggleChange(bool isOn) {
        if (isOn) {
            uiBind.backStoryPanel.SetActive(true);
            uiBind.gameRulePanel.SetActive(false);
            uiBind.characterInfoPanel.SetActive(false);
        } else {
            uiBind.backStoryPanel.SetActive(false);
        }
    }

    private void OnGameRuleToggleChange(bool isOn) {
        if (isOn) {
            uiBind.gameRulePanel.SetActive(true);
            uiBind.backStoryPanel.SetActive(false);
            uiBind.characterInfoPanel.SetActive(false);
        } else {
            uiBind.gameRulePanel.SetActive(false);
        }
    }

    private void OnCharacterInfoToggleChange(bool isOn) {
        if (isOn) {
            uiBind.characterInfoPanel.SetActive(true);
            uiBind.backStoryPanel.SetActive(false);
            uiBind.gameRulePanel.SetActive(false);
        } else {
            uiBind.characterInfoPanel.SetActive(false);
        }
    }


    private void OnGetBookDetailRespond(MFRespondHeader header, MFGetBookDetailRespond data) {
        if(header.result == 0) {
            MFGameObjectUtil.Find<Text>(uiBind.backStoryPanel, "Content").text = data.backStory;
        }
    }
}
