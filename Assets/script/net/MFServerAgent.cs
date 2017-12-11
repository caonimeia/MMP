using System;
using System.Collections.Generic;

public class Protocol {
    public int id;
    public string name;
}

public static class MFServerAgent {
    private static Dictionary<int, Protocol> protocolDic = new Dictionary<int, Protocol>();
    private static Dictionary<MFProtocolId, Action<string>> respondCallBackDic = new Dictionary<MFProtocolId, Action<string>>();
    public static void InitProtocolList() {
        List<Protocol> protocolList = MFTabFileReader.LoadTabFile<Protocol>("protocol/protocol.tab");
        foreach (Protocol item in protocolList) {
            protocolDic.Add(item.id, item);
        }
    }

    public static void Init() {
        respondCallBackDic.Add(MFProtocolId.qqLoginRespond, OnQQLoginRespond);
    }

    public static void RegisterRpcCallBack<T>(MFProtocolId protocolId, Action<T> callBack) {
        Messenger<T>.AddListener(protocolId.ToString(), callBack);
    }

    public static void InvokeRpcCallBack(MFProtocolId protocolId, string data) {
        Action<string> action;
        if(respondCallBackDic.TryGetValue(protocolId, out action)) {
            action(data);
        }
    }

    private static void DoRequest<T>(T arg) {
        string data = MFJsonSerialzator.Serialize(arg);
        MFNetManager.GetInstance().Send(data);
    }

    #region QQ登录
    public static void DoQQLoginRequest(int playerId) {
        DoRequest(new MFRequestProtocol<MFQQLoginRequest> {
            protocolId = MFProtocolId.qqLoginRequest,
            data = new MFQQLoginRequest {
                playerId = 10000,
            },
        });
    }

    public static void OnQQLoginRespond(string data) {
        MFQQLoginRespond taq = MFJsonSerialzator.DeSerialize<MFQQLoginRespond>(data);
        Messenger<MFQQLoginRespond>.Broadcast(taq.protocolId.ToString(), taq);
    }
    #endregion
}