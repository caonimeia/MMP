public class MFJoinRoomBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.joinPrepareRoomRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.joinPrepareRoomRespond, this);
    }
}

public class MFMockJoinRoom : MFJoinRoomBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"data\": {\"roomNumber\": 1002, \"scriptId\": \"144528290592600064\", \"playerCount\" : 4, \"userList\": [{\"icon\": \"\", \"isReady\": false, \"isRoomOwner\": false, \"level\": 1, \"murdererLoseRate\": \"40%\", \"murdererWinRate\": \"50%\", \"name\": \"测试账号\", \"playScriptNumber\": 0, \"point\": 100, \"roomRole\": -1, \"winRate\": \"60%\"}, {\"icon\": \"\", \"isReady\": false, \"isRoomOwner\": false, \"level\": 1, \"murdererLoseRate\": \"40%\", \"murdererWinRate\": \"50%\", \"name\": \"测试账号\", \"playScriptNumber\": 0, \"point\": 100, \"roomRole\": -1, \"winRate\": \"60%\"} ] }, \"header\": {\"protocolId\": 3001, \"result\": 0 } }";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFJoinRoomRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFJoinRoomRespond>>(data);
        MFPrepareRoomView.OnJoinRoomRespond(rp.header, rp.data);
    }
}

public class MFServerJoinRoom : MFJoinRoomBase {
    public override void Request(MFProtocolId id, params object[] args) {
        int roomId = (int)args[0];

        var package = new MFRequestProtocol<MFJoinRoomRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFJoinRoomRequest {
                roomNumber = roomId,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFJoinRoomRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFJoinRoomRespond>>(data);
        if(rp.data.refreshPage == 0) {
            MFPrepareRoomView.OnJoinRoomRespond(rp.header, rp.data);
            return;
        }

        MFUIMgr.GetUiInstance<MFPrepareRoomView>().RefreshPlayerList(rp.data.userList);
    }
}
