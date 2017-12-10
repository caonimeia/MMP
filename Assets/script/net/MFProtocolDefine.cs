using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public enum MFProtocolDefine {
    none = -1,
    connect = 0,
    test,
    login,
}

[Serializable]
public class ProtocolHeader {
    public MFProtocolDefine protocolId;
}

[Serializable]
public class TestA {
    public int a;
    public int[] b;
}

[Serializable]
public class TestARequest : ProtocolHeader {
    public int playerId;
}

[Serializable]
public class TestARespond : ProtocolHeader {
    public int a;
    public int[] b;
}