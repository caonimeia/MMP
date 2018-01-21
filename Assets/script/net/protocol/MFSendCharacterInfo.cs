using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFSendCharacterScriptBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.sendCharacterScriptRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.sendCharacterScriptRespond, this);
    }
}

public class MFMockSendCharacterScript : MFSendCharacterScriptBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"data\":{\"script\":\"角色故事1\"},\"header\":{\"broadcast\":0,\"protocolId\":3009,\"result\":0}}";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFSendCharacterScriptRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFSendCharacterScriptRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnSendCharacterScriptRespond(rp.header, rp.data);
    }
}

public class MFServerSendCharacterScript : MFSendCharacterScriptBase {
    public override void Request(MFProtocolId id, params object[] args) {
        int roomNumber = (int)args[0];

        var package = new MFRequestProtocol<MFSendCharacterScriptRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFSendCharacterScriptRequest {
                roomNumber = roomNumber,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFSendCharacterScriptRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFSendCharacterScriptRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnSendCharacterScriptRespond(rp.header, rp.data);
    }
}

