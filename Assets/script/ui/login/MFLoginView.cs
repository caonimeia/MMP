using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using cn.sharesdk.unity3d;

public class MFLoginView : MFUIBase {
    private MFLoginViewBind uiBind;

    protected override void Awake() {
        base.Awake();
        uiBind = GetComponent<MFLoginViewBind>();
        Assert.IsNotNull(uiBind);
        AddBtnListener();

        MFServerAgent.RegisterRpcCallBack<MFQQLoginRespond>(MFProtocolId.qqLoginRespond, OnQQLoginRespond);
        GameAgent.ssdk.authHandler = AuthResultHandler;
    }

    void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result) {
        if (state == ResponseState.Success) {
            print("authorize success !");
        } else if (state == ResponseState.Fail) {
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
        } else if (state == ResponseState.Cancel) {
            print("cancel !");
        }
    }

    private void AddBtnListener() {
        uiBind.qqLoginBtn.onClick.AddListener(OnQQLoginBtnClick);
        uiBind.wechatLoginBtn.onClick.AddListener(OnWeChatLoginBtnClick);
    }

    private void OnQQLoginBtnClick() {
        GameAgent.ssdk.Authorize(PlatformType.QQPlatform);
        //MFServerAgent.DoQQLoginRequest(10086);
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
