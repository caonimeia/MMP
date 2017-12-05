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
    }

    private void AddBtnListener() {
        uiBind.qqLoginBtn.onClick.AddListener(OnQQLoginBtnClick);
        uiBind.wechatLoginBtn.onClick.AddListener(OnWeChatLoginBtnClick);
    }

    private void OnQQLoginBtnClick() {
        StartCoroutine(LoadMainScene());
    }

    private void OnWeChatLoginBtnClick() {

    }

    private IEnumerator LoadMainScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
        while(!asyncLoad.isDone)
            yield return null;

        MFUIMgr.Close<MFLoginView>();
        MFUIMgr.Open<MFMainView>();
    }
}
