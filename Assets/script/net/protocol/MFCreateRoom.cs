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
        string data = "{\"data\": {\"roomNumber\": 1002, \"scriptId\": \"144528290592600064\", \"playerCount\" : 4, \"userList\": [{\"icon\": \"\", \"isReady\": false, \"isRoomOwner\": true, \"level\": 1, \"murdererLoseRate\": \"40%\", \"murdererWinRate\": \"50%\", \"name\": \"测试账号\", \"playScriptNumber\": 0, \"point\": 100, \"roomRole\": -1, \"winRate\": \"60%\"}, {\"icon\": \"\", \"isReady\": false, \"isRoomOwner\": false, \"level\": 1, \"murdererLoseRate\": \"40%\", \"murdererWinRate\": \"50%\", \"name\": \"测试账号\", \"playScriptNumber\": 0, \"point\": 100, \"roomRole\": -1, \"winRate\": \"60%\"} ] }, \"header\": {\"protocolId\": 3001, \"result\": 0 } }";
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
