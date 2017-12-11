using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MFLoginView : MFUIBase {
    private MFLoginViewBind uiBind;

    protected override void Awake() {
        base.Awake();
        uiBind = GetComponent<MFLoginViewBind>();
        Assert.IsNotNull(uiBind);
        AddBtnListener();

        MFServerAgent.RegisterRpcCallBack<MFQQLoginRespond>(MFProtocolId.qqLoginRespond, OnQQLoginRespond);
    }

    private void AddBtnListener() {
        uiBind.qqLoginBtn.onClick.AddListener(OnQQLoginBtnClick);
        uiBind.wechatLoginBtn.onClick.AddListener(OnWeChatLoginBtnClick);
    }

    private void OnQQLoginBtnClick() {
        MFServerAgent.DoQQLoginRequest(10086);
    }

    private void OnWeChatLoginBtnClick() {

    }

    private void OnQQLoginRespond(MFQQLoginRespond data) {
        MFPlayer player = new MFPlayer(data.playerId, data.playerName, data.playerLevel);
        StartCoroutine(LoadMainScene(player));
    }

    private IEnumerator LoadMainScene(MFPlayer player) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
        while(!asyncLoad.isDone)
            yield return null;

        MFUIMgr.Close<MFLoginView>();
        MFMainView.Open(player);
    }
}
