using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFGetCharacterListBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.getCharacterListRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.getCharacterListRespond, this);
    }
}

public class MFMockGetCharacterList : MFGetCharacterListBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"data\":{\"roleList\":[{\"codeId\":\"123456789\",\"desc\":\"男角色\",\"isSelect\":1,\"roleId\":0,\"sex\":1},{\"codeId\":\"1234567890\",\"desc\":\"女角色\",\"isSelect\":0,\"roleId\":1,\"sex\":2}]},\"header\":{\"broadcast\":0,\"protocolId\":3005,\"result\":0}}";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFGetCharacterListRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFGetCharacterListRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnGetCharacterListRespond(rp.header, rp.data);
    }
}

public class MFServerGetCharacterList : MFGetCharacterListBase {
    public override void Request(MFProtocolId id, params object[] args) {
        int roomNumber = (int)args[0];

        var package = new MFRequestProtocol<MFGetCharacterListRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFGetCharacterListRequest {
                roomNumber = roomNumber,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFGetCharacterListRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFGetCharacterListRespond>>(data);
        MFUIMgr.GetUiInstance<MFPrepareRoomView>().OnGetCharacterListRespond(rp.header, rp.data);
    }
}

