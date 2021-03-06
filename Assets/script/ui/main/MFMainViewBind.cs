﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MFMainViewBind : MonoBehaviour {
    public GameObject bookPanel;
    public GameObject bookListContentView;
    public GameObject bookTemplate;
    public Toggle bookListToggle;

    public GameObject msgPanel;
    public GameObject msgListContentView;
    public GameObject msgTemplate;
    public Toggle msgListToggle;

    public Image playerAvatar;
    public Text playerName;
    public Text playerLevel;

    public Button joinRoomBtn;
    public InputField roomNumberInput;
    public Button joinRoomConfirmBtn;
    public Button testEntrance;
}
