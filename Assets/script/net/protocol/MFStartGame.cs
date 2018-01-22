using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFStartGameBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.startGameRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.startGameRespond, this);
    }
}

public class MFMockStartGame : MFStartGameBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"data\":{\"script\":\"角色故事1\"},\"header\":{\"broadcast\":0,\"protocolId\":3009,\"result\":0}}";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFStartGameRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFStartGameRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnStartGameRespond(rp.header, rp.data);
    }
}

public class MFServerStartGame : MFStartGameBase {
    public override void Request(MFProtocolId id, params object[] args) {
        int roomNumber = (int)args[0];

        var package = new MFRequestProtocol<MFStartGameRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFStartGameRequest {
                roomNumber = roomNumber,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFStartGameRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFStartGameRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnStartGameRespond(rp.header, rp.data);
    }
}

