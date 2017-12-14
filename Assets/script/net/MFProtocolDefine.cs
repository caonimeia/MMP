using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public enum MFProtocolId {
    none = -1,
    connect = 0,
    qqLoginRequest, 
    qqLoginRespond,

    getPlayerBookListRequest,
    getPlayerBookListRespond,

    getBookDetailRequest,
    getBookDetailRespond,

    createRoomRequest,
    createRoomRespond,

    joinPrepareRoomRequest,
    joinPrepareRoomRespond,

    startGameRequest,
    startGameRespond,
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
    public int playerId;
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
    public MFBookInfo bookInfo;
    public List<MFPlayerInfo> playerList;
}
#endregion
