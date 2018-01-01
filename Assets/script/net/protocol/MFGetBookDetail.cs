public class MFGetBookDetailBase : MFProtocolReg, MFProtocolAction {
    public virtual void Request(MFProtocolId id, params object[] args) {

    }

    public virtual void Respond(string data) {

    }

    protected override void Register() {
        MFServerAgentBase.RegisterRequest(MFProtocolId.getBookDetailRequest, this);
        MFServerAgentBase.RegisterRespond(MFProtocolId.getBookDetailRespond, this);
    }
}

public class MFMockGetBookDetail : MFGetBookDetailBase {
    public override void Request(MFProtocolId id, params object[] args) {
        string data = "{\"header\": {\"protocolId\": 6, \"result\": 0, \"errMsg\": \"\"}, \"data\": {\"backStory\": \"aaaa\\n aaaa\"} }";
        Respond(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFGetBookDetailRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFGetBookDetailRespond>>(data);
        MFUIMgr.GetUiInstance<MFBookView>().OnGetBookDetailRespond(rp.header, rp.data);
    }
}

public class MFServerGetBookDetail : MFGetBookDetailBase {
    public override void Request(MFProtocolId id, params object[] args) {
        int bookId = (int)args[0];

        var package = new MFRequestProtocol<MFGetBookDetailRequest> {
            header = new MFRequestHeader {
                protocolId = id,
            },
            data = new MFGetBookDetailRequest {
                bookId = bookId,
            },
        };

        string data = MFJsonSerialzator.Serialize(package);
        MFNetManager.GetInstance().Send(data);
    }

    public override void Respond(string data) {
        MFRespondProtocol<MFGetBookDetailRespond> rp = MFJsonSerialzator.DeSerialize<MFRespondProtocol<MFGetBookDetailRespond>>(data);
        MFUIMgr.GetUiInstance<MFBookView>().OnGetBookDetailRespond(rp.header, rp.data);
    }
}
