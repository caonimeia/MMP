using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFSelectCharacterBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.selectCharacterRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.selectCharacterRespond, this);
    }
}

public class MFMockSelectCharacter : MFSelectCharacterBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"data\":{\"result\":true,\"roleList\":[{\"codeId\":\"123456789\",\"desc\":\"男角色\",\"isSelect\":1,\"roleId\":0,\"sex\":1},{\"codeId\":\"1234567890\",\"desc\":\"女角色\",\"isSelect\":1,\"roleId\":1,\"sex\":2}]},\"header\":{\"broadcast\":0,\"protocolId\":3007,\"result\":0}}";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFSelectCharacterRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFSelectCharacterRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnSelectCharacterRespond(rp.header, rp.data);
    }
}

public class MFServerSelectCharacter : MFSelectCharacterBase {
    public override void Request(MFProtocolId id, params object[] args) {
        int roleId = (int)args[0];
        int roomNumber = (int)args[1];

        var package = new MFRequestProtocol<MFSelectCharacterRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFSelectCharacterRequest {
                roleId = roleId,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFSelectCharacterRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFSelectCharacterRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnSelectCharacterRespond(rp.header, rp.data);
    }
}

