using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAgent : MonoBehaviour {
    private void Awake() {
        MFUIMgr.Init();
        GameObject.DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start() {
        MFUIMgr.Open<MFLoginView>();
    }

    // Update is called once per frame
    private void Update() {
        MFTimer.Update();
    }

    private void FixedUpdate() {

    }

    private void OnDestroy() {

    }
}
