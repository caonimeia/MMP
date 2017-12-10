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

    public void Send(string data) {
        _socketClient.Send(data);
    }

    public void Receive() {
        while (_recvQueue.Count > 0) {
            string data = _recvQueue.Dequeue();
            DispatchRespond(data);
        }
    }

    private void DispatchRespond(string data) {
        ProtocolHeader ph = MFJsonSerialzator.DeSerialize<ProtocolHeader>(data);
        MFServerAgent.InvokeRpcCallBack(ph.protocolId, data);
    }

    // Unity不允许在主线程之外访问GameObject和Unity的接口 所以加入一个队列 由主线程在Update中调用
    public void PushRecvData(string data) {
        _recvQueue.Enqueue(data);
    }

    public void Update() {
        Receive();
    }
}


