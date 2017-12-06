using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MFBookView : MFUIBase {
    private MFBookViewBind uiBind;
    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFBookViewBind>();
        Assert.IsNotNull(uiBind);
    }

    protected override void Start() {
        base.Start();

        uiBind.backBtn.onClick.AddListener(OnClickBackBtn);
        
    }

    protected override void OnEnable() {
        base.OnEnable();

        AddToggleListener();
        uiBind.backStoryToggle.Select();
        uiBind.backStoryToggle.isOn = true;
    }

    protected override void OnDisable() {
        base.OnDisable();
        RemoveToggleListener();
    }

    private void OnClickBackBtn() {
        MFUIMgr.Close<MFBookView>();
        MFUIMgr.Open<MFMainView>();
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
}
