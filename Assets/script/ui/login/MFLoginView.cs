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

    private void OnQQLoginRespond(MFRespondHeader header, MFQQLoginRespond data) {
        if(header.result == 0) {
            MFPlayer player = new MFPlayer(data.playerInfo);
            GameAgent.curPlayer = player;
            List<MFBook> bookList = new List<MFBook>();
            foreach(MFBookInfo bookInfo in data.bookList) {
                bookList.Add(new MFBook(bookInfo));
            }
            StartCoroutine(LoadMainScene(player, bookList));
        } else {
            
        }
    }

    private IEnumerator LoadMainScene(MFPlayer player, List<MFBook> bookList) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
        while(!asyncLoad.isDone)
            yield return null;

        MFUIMgr.Close<MFLoginView>();
        MFMainView.Open(player, bookList);
    }
}
