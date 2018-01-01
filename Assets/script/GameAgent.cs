using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.sharesdk.unity3d;

public class GameAgent : MonoBehaviour {
    public bool debug = true;
    private MFNetManager _netMgr = MFNetManager.GetInstance();
    public static MFPlayer curPlayer;
    public static ShareSDK ssdk;

    private void Awake() {
        MFUIMgr.Init();
        
#if UNITY_EDITOR
        if (!debug)
            _netMgr.Init(new MFSocketClient("116.196.109.146", 8090));
#else
        if (debug)
            _netMgr.Init(new MFSocketClient("10.0.2.2", 8090));
        else
            _netMgr.Init(new MFSocketClient("192.168.0.101", 8090));

        MFAgoraMgr.Init();
#endif

        ssdk = GetComponent<ShareSDK>();

        MFApplicationUtil.SetDebugMode(debug);

        MFServerAgentBase.Init();

        DontDestroyOnLoad(gameObject);

        if (debug)
            MFLog.LogInfo("Debug Mode Start");
    }

    // Use this for initialization
    private void Start() {
        if (!debug)
            MFNetManager.GetInstance().Connect();

        MFUIMgr.Open<MFLoginView>();
    }

    // Update is called once per frame
    private void Update() {
        MFNetManager.GetInstance().Update();
        MFTimer.Update();
    }

    private void FixedUpdate() {

    }

    private void OnDestroy() {
        if (!debug)
            MFNetManager.GetInstance().DisConnect();
    }
}
