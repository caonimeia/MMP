using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFGameRoomView : MFUIBase {
    public static void Open() {
        MFUIMgr.Open<MFGameRoomView>();
    }

    private MFGameRoomBind uiBind;

    protected override void Awake() {
        base.Awake();
    }
}
