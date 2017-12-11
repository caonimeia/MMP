using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFPlayer {
    private int _id;
    private string _name;
    private int _level;

    public MFPlayer(int id, string name, int level) {
        _id = id;
        _name = name;
        _level = level;
    }

    public string GetName() {
        return _name;
    }

    public int GetLevel() {
        return _level;
    }
}
