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

    getBookBackStoryRequest,
    getBookBackStoryRespond,

    createRoomRequest,
    createRoomRespond,

    joinPrepareRoomRequest,
    joinPrepareRoomRespond,

    startGameRequest,
    startGameRespond,
}

[Serializable]
public class MFRequestProtocol<T> {
    public MFProtocolId protocolId;
    public T data;
}

[Serializable]
public class MFRespondProtocol {
    public MFProtocolId protocolId;
    public int result;
    public string errMsg;
}

[Serializable]
public class MFQQLoginRequest {
    public int playerId;
}

[Serializable]
public class MFQQLoginRespond : MFRespondProtocol {
    public int playerId;
    public string playerName;
    public int playerLevel;

    public MFBook[] bookList;
}