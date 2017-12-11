using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool CanOpen() {
        if (IsFree())
            return true;

        return _isBuy;
    }

    public bool IsFree() {
        return _price <= 0;
    }
}
