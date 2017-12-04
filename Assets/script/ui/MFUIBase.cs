using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MFUIBase : UIBehaviour {
    protected override void Awake() {
        base.Awake();
    }

    protected override void Start () {
        base.Start();
	}

    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }

    protected override void OnDestroy() {
        base.OnDestroy();
    }

    protected virtual void Update() {

    }

    protected virtual void OnShow() {

    }

    protected virtual void OnClose() {

    }
}
