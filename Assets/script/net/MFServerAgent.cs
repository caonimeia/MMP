using System;


public static class MFServerAgent {
    public static void RegisterRpcCallBack(MFProtocolDefine protocolId, Action<string> callBack) {
        Messenger<string>.AddListener(protocolId.ToString(), callBack);
    }

    public static void InvokeRpcCallBack(MFProtocolDefine protocolId, string data) {
        Messenger<string>.Broadcast(protocolId.ToString(), data);
    }

    public static void DoRequest<T>(T arg) where T : ProtocolHeader {
        string data = MFJsonSerialzator.Serialize(arg);
        MFNetManager.GetInstance().Send(data);
    }
}