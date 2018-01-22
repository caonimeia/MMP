using System;
using System.Collections.Generic;

public class Protocol {
    public int id;
    public string name;
}

public class MFServerAgentBase {
    public static Dictionary<MFProtocolId, MFProtocolAction> requestDic = new Dictionary<MFProtocolId, MFProtocolAction>();
    public static Dictionary<MFProtocolId, MFProtocolAction> respondDic = new Dictionary<MFProtocolId, MFProtocolAction>();

    public static void Init() {
        //初始化协议实例
        ProtocolList.Init();
    }

    public static void RegisterRequest(MFProtocolId id, MFProtocolAction actionClass) {
        if (requestDic.ContainsKey(id)) {
            MFLog.LogError("请求协议重复注册：", id);
            return;
        }
        requestDic.Add(id, actionClass);
    }

    public static void RegisterRespond(MFProtocolId id, MFProtocolAction p) {
        if (respondDic.ContainsKey(id)) {
            MFLog.LogError("响应协议重复注册：", id);
            return;
        }
        respondDic.Add(id, p);
    }

    public static void Send(MFProtocolId id, params object[] args) {
        MFProtocolAction actionClass;
        if(!requestDic.TryGetValue(id, out actionClass)) {
            MFLog.LogError("请求协议未注册：", id);
            return;
        }
        
        actionClass.Request(id, args);
    }

    public static void Receive(MFProtocolId id, string data) {
        MFProtocolAction actionClass;
        if (!respondDic.TryGetValue(id, out actionClass)) {
            MFLog.LogError("响应协议未注册：", id);
            return;
        }

        actionClass.Respond(data);
    }
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
        //respondCallBackDic.Add(MFProtocolId.heartbeatRespond, OnHeartBeatRespond);
        //respondCallBackDic.Add(MFProtocolId.qqLoginRespond, OnQQLoginRespond);
        //respondCallBackDic.Add(MFProtocolId.getBookDetailRespond, OnGetBookDetailRespond);
        //respondCallBackDic.Add(MFProtocolId.createRoomRespond, OnCreateRoomRespond);
        //respondCallBackDic.Add(MFProtocolId.readyToStartRespond, OnReadyToStartRespond);
        
    }

    public static void InvokeRpcCallBack(MFProtocolId protocolId, string data) {
        Action<string> action;
        if(respondCallBackDic.TryGetValue(protocolId, out action)) {
            action(data);
        }
    }

    public static void DoRequest<T>(T arg) {
        string data = MFJsonSerialzator.Serialize(arg);
        MFNetManager.GetInstance().Send(data);
    }

    #region 心跳包
    private static void OnHeartBeatRespond(string data) {
        MFLog.LogInfo("heartbeat");
    }
    #endregion

    #region QQ登录
    public static void DoQQLoginRequest(int playerId) {
        DoRequest(new MFRequestProtocol<MFQQLoginRequest> {
            header = new MFRequestHeader {
                protocolId = MFProtocolId.qqLoginRequest,
            },
            data = new MFQQLoginRequest {
                playerId = playerId,
                type = PlatformTypeDebug.win,
            },
        });
    }

    public static void OnQQLoginRespond(string data) {
        MFRespondProtocol<MFQQLoginRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFQQLoginRespond>>(data);
        MFUIMgr.GetUiInstance<MFLoginView>().OnQQLoginRespond(rp.header, rp.data);
    }
    #endregion

    #region 获取本子背景故事
    public static void DoGetBookDetailRequest(int bookId) {
        DoRequest(new MFRequestProtocol<MFGetBookDetailRequest> {
            header = new MFRequestHeader {
                protocolId = MFProtocolId.getBookDetailRequest,
            },
            data = new MFGetBookDetailRequest {
                bookId = bookId,
            },
        });
    }

    public static void OnGetBookDetailRespond(string data) {
        MFRespondProtocol<MFGetBookDetailRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFGetBookDetailRespond>>(data);
        MFUIMgr.GetUiInstance<MFBookView>().OnGetBookDetailRespond(rp.header, rp.data);
    }
    #endregion

    #region 创建房间
    public static void DoCreateRoomRequest(int playerId, int bookId) {
        DoRequest(new MFRequestProtocol<MFCreateRoomRequest> {
            header = new MFRequestHeader {
                protocolId = MFProtocolId.createRoomRequest,
            },
            data = new MFCreateRoomRequest {
                //playerId = playerId,
                //bookId = bookId,
            },
        });
    }

    public static void OnCreateRoomRespond(string data) {
        MFRespondProtocol<MFCreateRoomRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFCreateRoomRespond>>(data);
        MFUIMgr.GetUiInstance<MFBookView>().OnCreateRoomRespond(rp.header, rp.data);
    }
    #endregion

    #region 玩家准备
    //public static void DoReadyToStartRequest(int roomId, int playerId) {
    //    DoRequest(new MFRequestProtocol<MFReadyToStartRequest> {
    //        header = new MFRequestHeader {
    //            protocolId = MFProtocolId.readyToStartRequest,
    //        },
    //        data = new MFReadyToStartRequest {
    //            roomId = roomId,
    //            playerId = playerId,
    //        },
    //    });
    //}

    //public static void OnReadyToStartRespond(string data) {
    //    MFRespondProtocol<MFReadyToStartRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFReadyToStartRespond>>(data);
    //    MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnReadyToStartRespond(rp.header, rp.data);
    //}
    #endregion
}