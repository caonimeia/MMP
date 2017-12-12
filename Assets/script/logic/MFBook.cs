using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct MFBookInfo {
    public int id;
    public string name;
    public int playerCount;
    public float price;
    public bool isBuy;
}

public class MFBook {
    private int _id;
    private string _name;
    private int _playerCount;
    private string _backStory;
    private float _price;
    private bool _isBuy;

    public MFBook(int id, string name, int playerCount) {
        _id = id;
        _name = name;
        _playerCount = playerCount;
    }

    public MFBook(MFBookInfo info) {
        _id = info.id;
        _name = info.name;
        _playerCount = info.playerCount;
        _price = info.price;
        _isBuy = info.isBuy;
    }

    public bool CanOpen() {
        if (IsFree())
            return true;

        return _isBuy;
    }

    public bool IsFree() {
        return _price <= 0;
    }
}
