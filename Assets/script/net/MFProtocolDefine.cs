using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public enum MFProtocolId {
    none = -1,
    connect = 0,
    qqLoginRequest, 
    qqLoginRespond,

    heartbeatRespond,
    heartbeatRequest,

    getBookDetailRequest,
    getBookDetailRespond,

    createRoomRequest,
    createRoomRespond,

    joinPrepareRoomRequest,
    joinPrepareRoomRespond,

    startGameRequest,
    startGameRespond,

    readyToStartRequest,
    readyToStartRespond,
}

public interface MFProtocolAction {
    void Request(MFProtocolId id, params object[] args);
    void Respond(string data);
}

public abstract class MFProtocolReg {
    public MFProtocolReg() {
        Register();
    }

    protected abstract void Register();
}

public static class ProtocolList {
    public static List<MFProtocolReg> list = new List<MFProtocolReg>();

    public static void Init() {
        if (MFApplicationUtil.IsOpenDebug())
            InitDebugList();
        else
            InitReleaseList();
    }

    private static void InitDebugList() {
        list.Add(new MFMockQQLogin());
        list.Add(new MFMockGetBookDetail());
    }

    private static void InitReleaseList() {
        list.Add(new MFServerQQLogin());
    }
}

[Serializable]
public class MFRequestHeader {
    public MFProtocolId protocolId;
}

[Serializable]
public class MFRequestProtocol<T> {
    public MFRequestHeader header;
    public T data;
}

[Serializable]
public class MFRespondHeader {
    public MFProtocolId protocolId;
    public int result;
    public string errMsg;
}

[Serializable]
public class MFRespondProtocol<T> {
    public MFRespondHeader header;
    public T data;
}


#region QQ登录
[Serializable]
public class MFQQLoginRequest {
    public string tokenId;
    public int playerId;
    public PlatformTypeDebug type;
}

[Serializable]
public class MFQQLoginRespond {
    public MFPlayerInfo playerInfo;
    public MFBookInfo[] bookList;
}
#endregion

#region 获取本子详细信息
[Serializable]
public class MFGetBookDetailRequest {
    public int bookId;
}

[Serializable]
public class MFGetBookDetailRespond {
    public string backStory;
    public string[] characterInfo;
}
#endregion

#region 创建房间
[Serializable]
public class MFCreateRoomRequest {
    public int playerId;
    public int bookId;
}

[Serializable]
public class MFCreateRoomRespond {
    public int roomId;
    public int roomMasterId;
    public MFBookInfo bookInfo;
    public List<MFPlayerInfo> playerList;
}
#endregion

#region 玩家准备
[Serializable]
public class MFReadyToStartRequest {
    public int roomId;
    public int playerId;
}

[Serializable]
public class MFReadyToStartRespond {

}
#endregion
