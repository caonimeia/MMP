using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFReadyToStartBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.readyToStartRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.readyToStartRespond, this);
    }
}

public class MFMockReadyToStart : MFReadyToStartBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"data\":{\"userList\":[{\"icon\":\"\",\"ready\":true,\"isRoomOwner\":false,\"level\":1,\"murdererLoseRate\":\"40%\",\"murdererWinRate\":\"50%\",\"name\":\"测试账号\",\"playScriptNumber\":0,\"point\":100,\"roomRole\":-1,\"winRate\":\"60%\"},{\"icon\":\"\",\"ready\":true,\"isRoomOwner\":false,\"level\":1,\"murdererLoseRate\":\"40%\",\"murdererWinRate\":\"50%\",\"name\":\"测试账号\",\"playScriptNumber\":0,\"point\":100,\"roomRole\":-1,\"winRate\":\"60%\"}]},\"header\":{\"protocolId\":3001,\"result\":0}}";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFReadyToStartRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFReadyToStartRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnReadyToStartRespond(rp.header, rp.data);
    }
}

public class MFServerReadyToStart : MFReadyToStartBase {
    public override void Request(MFProtocolId id, params object[] args) {
        int roomNumber = (int)args[0];

        var package = new MFRequestProtocol<MFReadyToStartRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFReadyToStartRequest {
                roomNumber = roomNumber,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFReadyToStartRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFReadyToStartRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnReadyToStartRespond(rp.header, rp.data);
    }
}

