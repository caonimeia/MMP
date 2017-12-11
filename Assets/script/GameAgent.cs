﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAgent : MonoBehaviour {
    private MFNetManager _netMgr = MFNetManager.GetInstance();
    private void Awake() {
        MFUIMgr.Init();
        _netMgr.Init(new MFSocketClient("127.0.0.1", 2333));
        //_netMgr.Init(new MFSocketClient("192.168.0.107", 8090));
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start() {
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
        MFNetManager.GetInstance().DisConnect();
    }
}
