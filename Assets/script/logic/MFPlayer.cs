using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MFPlayerInfo {
    public int id;
    public string name;
    public int level;
}

public class MFPlayer {
    private int _id;
    private string _name;
    private int _level;

    public MFPlayer(int id, string name, int level) {
        _id = id;
        _name = name;
        _level = level;
    }

    public MFPlayer(MFPlayerInfo info) {
        _id = info.id;
        _name = info.name;
        _level = info.level;
    }

    public string GetName() {
        return _name;
    }

    public int GetLevel() {
        return _level;
    }

    public int GetId() {
        return _id;
    }
}
