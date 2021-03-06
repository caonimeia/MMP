﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MFPlayerInfo {
    public string id;
    public string name;
    public int level;
    public string icon;
    public string murdererLoseRate;
    public string murdererWinRate;
    public int playScriptNumber;
    public int point;
    public string winRate;
    public string codeId;
}

[Serializable]
public class MFPrepareRoomPlayerInfo : MFPlayerInfo {
    public bool ready;
    public int roomRole;
}

public class MFPlayer {
    private string _id;
    private string _name;
    private int _level;
    private string _codeId;

    public MFPlayer(string id, string name, int level) {
        _id = id;
        _name = name;
        _level = level;
    }

    public MFPlayer(MFPlayerInfo info) {
        _id = info.id;
        _name = info.name;
        _level = info.level;
        _codeId = info.codeId;
    }

    public string GetName() {
        return _name;
    }

    public int GetLevel() {
        return _level;
    }

    public string GetId() {
        return _id;
    }

    public string GetCodeId() {
        return _codeId;
    }
}
