public class MFCreateRoomBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.createRoomRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.createRoomRespond, this);
    }
}

public class MFMockCreateRoom : MFCreateRoomBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"header\": {\"protocolId\": 6, \"result\": 0, \"errMsg\": \"\"}, \"data\": {\"backStory\": \"aaaa\\n aaaa\"} }";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFCreateRoomRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFCreateRoomRespond>>(data);
        MFUIMgr.GetUiInstance<MFBookView>().OnCreateRoomRespond(rp.header, rp.data);
    }
}

public class MFServerCreateRoom : MFCreateRoomBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string scriptId = args[0] as string;

        var package = new MFRequestProtocol<MFCreateRoomRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFCreateRoomRequest {
                scriptId = scriptId,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFCreateRoomRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFCreateRoomRespond>>(data);
        MFUIMgr.GetUiInstance<MFBookView>().OnCreateRoomRespond(rp.header, rp.data);
    }
}
