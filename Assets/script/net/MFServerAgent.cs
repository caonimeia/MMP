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
        respondCallBackDic.Add(MFProtocolId.heartbeatRespond, OnHeartBeatRespond);
        respondCallBackDic.Add(MFProtocolId.qqLoginRespond, OnQQLoginRespond);
        respondCallBackDic.Add(MFProtocolId.getBookDetailRespond, OnGetBookDetailRespond);
        respondCallBackDic.Add(MFProtocolId.createRoomRespond, OnCreateRoomRespond);
        respondCallBackDic.Add(MFProtocolId.readyToStartRespond, OnReadyToStartRespond);
        
    }

    public static void RegisterRpcCallBack<T>(MFProtocolId protocolId, Action<MFRespondHeader, T> callBack) {
        Messenger<MFRespondHeader, T>.AddListener(protocolId.ToString(), callBack);
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
            },
        });
    }

    public static void OnQQLoginRespond(string data) {
        MFRespondProtocol<MFQQLoginRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFQQLoginRespond>>(data);
        Messenger<MFRespondHeader, MFQQLoginRespond>.Broadcast(rp.header.protocolId.ToString(), rp.header, rp.data);
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
        Messenger<MFRespondHeader, MFGetBookDetailRespond>.Broadcast(rp.header.protocolId.ToString(), rp.header, rp.data);
    }
    #endregion

    #region 创建房间
    public static void DoCreateRoomRequest(int playerId, int bookId) {
        DoRequest(new MFRequestProtocol<MFCreateRoomRequest> {
            header = new MFRequestHeader {
                protocolId = MFProtocolId.createRoomRequest,
            },
            data = new MFCreateRoomRequest {
                playerId = playerId,
                bookId = bookId,
            },
        });
    }

    public static void OnCreateRoomRespond(string data) {
        MFRespondProtocol<MFCreateRoomRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFCreateRoomRespond>>(data);
        MFUIMgr.GetUiInstance<MFBookView>().OnCreateRoomRespond(rp.header, rp.data);
    }
    #endregion

    #region 玩家准备
    public static void DoReadyToStartRequest(int roomId, int playerId) {
        DoRequest(new MFRequestProtocol<MFReadyToStartRequest> {
            header = new MFRequestHeader {
                protocolId = MFProtocolId.readyToStartRequest,
            },
            data = new MFReadyToStartRequest {
                roomId = roomId,
                playerId = playerId,
            },
        });
    }

    public static void OnReadyToStartRespond(string data) {
        MFRespondProtocol<MFReadyToStartRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFReadyToStartRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnReadyToStartRespond(rp.header, rp.data);
    }
    #endregion
}