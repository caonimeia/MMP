using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct MFCharacterInfo {
    public string codeId; //用户ID
    public int roleId; //剧本角色ID
    public bool isSelect;
    public string desc;
}

public class MFCharacter {
    private int index;
    private string name;
    private string sex;

    public int Index {
        get {
            return index;
        }

        set {
            index = value;
        }
    }

    public string Name {
        get {
            return name;
        }

        set {
            name = value;
        }
    }

    public string Sex {
        get {
            return sex;
        }

        set {
            sex = value;
        }
    }
}
