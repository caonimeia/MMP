using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MFCharacterSelectView : MFUIBase {
    private MFCharacterSelectBind uiBind;
    

    public static void Open() {
        MFUIMgr.Open<MFCharacterSelectView>();
    }

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFCharacterSelectBind>();
        Assert.IsNotNull(uiBind);
    }

    protected override void Start() {
        base.Start();

    }

    protected override void OnShow() {
        base.OnShow();

    }
}
