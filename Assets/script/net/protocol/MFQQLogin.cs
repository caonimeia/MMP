public class MFQQLoginBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.qqLoginRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.qqLoginRespond, this);
    }
}

public class MFMockQQLogin : MFQQLoginBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"data\": {\"bookList\": [{\"id\": \"144528290592600064\", \"isBuy\": true, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 }, {\"id\": \"144528290592600064\", \"isBuy\": false, \"name\": \"荒古战争\", \"playerCount\": 4, \"price\": 0, \"quantity\": 100, \"type\": 0 } ], \"playerInfo\": {\"id\": \"144529366578376704\", \"level\": 1, \"name\": \"测试账号\"} }, \"header\": {\"protocolId\": 1, \"result\": 0 } }";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFQQLoginRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFQQLoginRespond>>(data);
        MFUIMgr.GetUiInstance<MFLoginView>().OnQQLoginRespond(rp.header, rp.data);
    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.qqLoginRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.qqLoginRespond, this);
    }
}

public class MFServerQQLogin : MFQQLoginBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string token = args[0] as string;

        var package = new MFRequestProtocol<MFQQLoginRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFQQLoginRequest {
                tokenId = token,
                type = PlatformTypeDebug.win,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFQQLoginRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFQQLoginRespond>>(data);
        MFUIMgr.GetUiInstance<MFLoginView>().OnQQLoginRespond(rp.header, rp.data);
    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.qqLoginRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.qqLoginRespond, this);
    }
}