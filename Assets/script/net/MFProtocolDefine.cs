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


    // 玩家创建房间
    createRoomRequest = 3000,
    createRoomRespond,

    // 玩家加入房间
    joinPrepareRoomRequest,
    joinPrepareRoomRespond,   // 广播给房间内的其他玩家

    // 玩家获取剧本列表
    getCharacterListRequest,
    getCharacterListRespond,

    // 玩家选择剧本
    selectCharacterRequest,
    selectCharacterRespond,   // 广播给房间内的其他玩家

    // 发放对应的剧本信息给玩家
    sendCharacterScriptRequest,
    sendCharacterScriptRespond,

    // 玩家准备
    readyToStartRequest,
    readyToStartRespond,     // 广播给房间内的其他玩家

    // 房主开始游戏
    startGameRequest,
    startGameRespond,        // 广播给房间内的其他玩家
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
        list.Add(new MFMockCreateRoom());
        list.Add(new MFMockJoinRoom());
        list.Add(new MFMockGetCharacterList());
        list.Add(new MFMockSelectCharacter());
        list.Add(new MFMockSendCharacterScript());
        list.Add(new MFMockReadyToStart());
        list.Add(new MFMockStartGame());
    }

    private static void InitReleaseList() {
        list.Add(new MFServerQQLogin());
        list.Add(new MFServerGetBookDetail());
        list.Add(new MFServerCreateRoom());
        list.Add(new MFServerJoinRoom());
        list.Add(new MFServerGetCharacterList());
        list.Add(new MFServerSelectCharacter());
        list.Add(new MFServerSendCharacterScript());
        list.Add(new MFServerReadyToStart());
        list.Add(new MFServerStartGame());
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
    public bool broadcast;
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
    public string scriptId;
}

[Serializable]
public class MFCreateRoomRespond {
    public string scriptId;
    public int roomNumber;
    public int playerCount;
    public string roomOwner;
    public List<MFPrepareRoomPlayerInfo> userList;
}
#endregion

#region 加入房间
[Serializable]
public class MFJoinRoomRequest {
    public int roomNumber;
}

[Serializable]
public class MFJoinRoomRespond {
    public string scriptId;
    public int roomNumber;
    public int playerCount;
    public int refreshPage;
    public string roomOwner;
    public List<MFPrepareRoomPlayerInfo> userList;
}
#endregion

#region 获取剧本角色列表
[Serializable]
public class MFGetCharacterListRequest {
    public int roomNumber;
}

[Serializable]
public class MFGetCharacterListRespond {
    public List<MFCharacterInfo> roleList;
}
#endregion

#region 选择剧本角色
[Serializable]
public class MFSelectCharacterRequest {
    public int roomNumber;
    public int roleId;
}

[Serializable]
public class MFSelectCharacterRespond {
    public bool result;
    public List<MFCharacterInfo> roleList;
}
#endregion

#region 房主发放剧本
[Serializable]
public class MFSendCharacterScriptRequest {
    public int roomNumber;
}

[Serializable]
public class MFSendCharacterScriptRespond {
    public string script;
}
#endregion


#region 玩家准备
[Serializable]
public class MFReadyToStartRequest {
    public int roomNumber;
}

[Serializable]
public class MFReadyToStartRespond {
    public List<MFPrepareRoomPlayerInfo> userList;
}
#endregion

#region 房主开始游戏
[Serializable]
public class MFStartGameRequest {
    public int roomNumber;
}

[Serializable]
public class MFStartGameRespond {

}
#endregion
