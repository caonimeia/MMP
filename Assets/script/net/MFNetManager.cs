using System;
using System.Collections.Generic;
using UnityEngine;

class MFNetManager {
    private MFSocketClient _socketClient;
    private readonly Queue<string> _recvQueue;
    private MFNetManager() {
        _recvQueue = new Queue<string>();
    }

    ~MFNetManager() {
        if (_socketClient != null) {
            _socketClient.DisConnect();
        }
    }

    private static MFNetManager _instance;
    public static MFNetManager GetInstance() {
        if (null == _instance) {
            _instance = new MFNetManager();
        }
        return _instance;
    }

    public void Init(MFSocketClient socketClient) {
        _socketClient = socketClient;
    }

    public void Connect() {
        _socketClient.Connect();
    }

    public void DisConnect() {
        _socketClient.DisConnect();
    }

    public void Send(string msg) {
        _socketClient.Send(msg);
    }

    public void Receive() {
        while (_recvQueue.Count > 0) {
            string data = _recvQueue.Dequeue();
            DispatchRespond(data);
        }
    }

    private void DispatchRespond(string data) {
        //todo 在这里解析协议 调用对应的回调函数
        ProtocolHeader ta = MFJsonSerialzator.DeSerialize<ProtocolHeader>(data);
        MFLog.LogInfo(ta.protocolId);
    }

    // Unity不允许在主线程之外访问GameObject和Unity的接口 所以加入一个队列 由主线程在Update中调用
    public void PushRecvData(string data) {
        _recvQueue.Enqueue(data);
    }

    public void Update() {
        Receive();
    }
}


