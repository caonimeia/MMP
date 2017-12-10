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

        MFServerAgent.RegisterRpcCallBack(MFProtocolDefine.test, OnQQLoginRespond);
    }

    private void AddBtnListener() {
        uiBind.qqLoginBtn.onClick.AddListener(OnQQLoginBtnClick);
        uiBind.wechatLoginBtn.onClick.AddListener(OnWeChatLoginBtnClick);
    }

    private void OnQQLoginBtnClick() {
        StartCoroutine(LoadMainScene());

        MFServerAgent.DoRequest(new TestARequest {
            protocolId = MFProtocolDefine.test,
            playerId = 10086,
        });
    }

    private void OnWeChatLoginBtnClick() {

    }

    private void OnQQLoginRespond(string data) {
        MFLog.LogInfo(data);
    }

    private IEnumerator LoadMainScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
        while(!asyncLoad.isDone)
            yield return null;

        MFUIMgr.Close<MFLoginView>();
        MFUIMgr.Open<MFMainView>();
    }
}
