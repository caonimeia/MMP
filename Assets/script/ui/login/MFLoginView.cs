using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using cn.sharesdk.unity3d;


public enum PlatformTypeDebug {
    win = 1,
    android = 2,
}

public class MFLoginView : MFUIBase {
    private MFLoginViewBind uiBind;

    protected override void Awake() {
        base.Awake();
        uiBind = GetComponent<MFLoginViewBind>();
        Assert.IsNotNull(uiBind);
        AddBtnListener();

        GameAgent.ssdk.authHandler = AuthResultHandler;
        GameAgent.ssdk.showUserHandler = OnGetUserInfo;
    }

    private void OnGetUserInfo(int reqID, ResponseState state, PlatformType type, Hashtable result){
        if (state == ResponseState.Success)
        {
            print ("get user info result :");
            print (MiniJSON.jsonEncode(result));
            print ("AuthInfo:" + MiniJSON.jsonEncode (GameAgent.ssdk.GetAuthInfo (PlatformType.QQ)));
            print ("Get userInfo success !Platform :" + type );
        }
        else if (state == ResponseState.Fail)
        {
            #if UNITY_ANDROID
            print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
            #elif UNITY_IPHONE
            print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
            #endif
        }
        else if (state == ResponseState.Cancel) 
        {
            print ("cancel !");
        }
    }

    void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result) {
        if (state == ResponseState.Success) {
            print("authorize success !");
            if (result != null && result.Count > 0) {
                print("authorize success !" + "Platform :" + type + "result:" + MiniJSON.jsonEncode(result));
            } else {
                print("authorize success !" + "Platform :" + type);
            }
            GameAgent.ssdk.GetUserInfo(PlatformType.QQPlatform);
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
#if UNITY_EDITOR
        MFServerAgentBase.Send(MFProtocolId.qqLoginRequest, "222222222222");
        //MFServerAgent.DoQQLoginRequest(10086);
#else
        GameAgent.ssdk.Authorize(PlatformType.QQPlatform);
#endif   
    }

    private void OnWeChatLoginBtnClick() {

    }

    public void OnQQLoginRespond(MFRespondHeader header, MFQQLoginRespond data) {
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
